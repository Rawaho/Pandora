using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Pandora.Cryptography;
using Pandora.Managers;
using Pandora.Network.Actions;
using Pandora.Network.Commands;

namespace Pandora.Network
{
    public delegate void CommandHandler(Session session, ClientCommand command);

    public static class NetworkManager
    {
        private static Thread listenerThread;

        private static readonly Dictionary<string, ClientCommandCached> clientCommandCache = new Dictionary<string, ClientCommandCached>();
        private static readonly Dictionary<Type, ServerActionCached> serverActionCache = new Dictionary<Type, ServerActionCached>();

        private static readonly List<ConnectionListener> listeners = new List<ConnectionListener>();

        public static void Initialise()
        {
            InitialiseClientCommands();
            InitialiseServerActions();

            try
            {
                listeners.Add(new ConnectionListener(2201, ServerType.WorldServer));
                listeners.Add(new ConnectionListener(2202, ServerType.GameServer));

                listenerThread = new Thread(() =>
                {
                    for (;;)
                        foreach (ConnectionListener listener in listeners)
                            listener.AcceptConnection();
                });

                listenerThread.Start();
            }
            catch
            {
                LogManager.Write("Network", "An exception occured while starting connection listener!");
                throw;
            }
        }

        private static void InitialiseClientCommands()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (ClientCommandAttribute command in type.GetCustomAttributes<ClientCommandAttribute>())
                {
                    var clientCommand = new ClientCommandCached(type);
                    foreach (PropertyInfo property in type.GetProperties())
                    {
                        ClientCommandFieldAttribute commandField = property.GetCustomAttribute<ClientCommandFieldAttribute>();
                        if (commandField != null)
                            clientCommand.Fields.Add(commandField.Field, property);
                    }

                    clientCommandCache.Add(command.Command, clientCommand);
                }
            }
        }

        private static void InitialiseServerActions()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (ServerActionAttribute action in type.GetCustomAttributes<ServerActionAttribute>())
                {
                    var serverAction = new ServerActionCached(action.Action);
                    foreach (PropertyInfo property in type.GetProperties())
                    {
                        ServerActionFieldAttribute actionField = property.GetCustomAttribute<ServerActionFieldAttribute>();
                        if (actionField != null)
                            serverAction.Fields.Add(new Tuple<ServerActionFieldAttribute, PropertyInfo>(actionField, property));
                    }

                    serverActionCache.Add(type, serverAction);
                }
            }
        }

        public static void MarshalPacket(Session session, string packet)
        {
            // command packet will always have at least 3 elements (seq, command, parameters...)
            string[] commandExplode = packet.TrimEnd('\n').Split('|');
            if (commandExplode.Length < 2)
                return;

            uint sequence;
            if (!uint.TryParse(commandExplode[0], out sequence))
                return;

            string command = commandExplode[1];

            string[] parameters = new string[commandExplode.Length - 2];
            Array.Copy(commandExplode, 2, parameters, 0, parameters.Length);

            // $@ is an encrypted transport packet, need to decryt first to obtain actual command
            if (command.StartsWith("$@"))
            {
                if (commandExplode.Length != 3)
                    return;

                try
                {
                    // decrypt and rebuild command to be parsed again
                    MarshalPacket(session, $"{commandExplode[0]}|{CryptoProvider.Decrypt(commandExplode[1].Remove(0, 2), commandExplode[2])}");
                }
                catch
                {
                    string source = session.Account?.Username ?? session.IpAddress;
                    LogManager.Write("Player", $"Malformed packet from {source}, failed to decrypt!");
                }
            }
            else
            {
                ClientCommandCached clientCommandCached;
                if (!clientCommandCache.TryGetValue(command, out clientCommandCached))
                {
                    LogManager.Write("Network", $"Received unknown packet {command}!");
                    return;
                }

                object instance = Activator.CreateInstance(clientCommandCached.Command);
                for (uint i = 2; i < commandExplode.Length; i++)
                {
                    // parameter will always have 2 values (key, element)
                    var parameter = commandExplode[i].Split(':');
                    if (parameter.Length != 2)
                        return;

                    PropertyInfo property;
                    if (!clientCommandCached.Fields.TryGetValue(parameter[0], out property))
                    {
                        LogManager.Write("Network", $"Unhandled element '{parameter[0]}' for command '{command}'!");
                        continue;
                    }

                    if (property.CanWrite)
                        property.SetValue(instance, Convert.ChangeType(parameter[1], property.PropertyType, null));
                }

                ((ClientCommand)instance).Handle(session);
            }
        }

        public static string DeMarshal(ServerAction serverAction)
        {
            ServerActionCached serverActionCached;
            if (!serverActionCache.TryGetValue(serverAction.GetType(), out serverActionCached))
                return string.Empty;

            var actionString = new StringBuilder(serverActionCached.Action);
            foreach (Tuple<ServerActionFieldAttribute, PropertyInfo> field in serverActionCached.Fields.OrderBy(f => f.Item1.Weight))
                if (field.Item2.CanRead)
                    actionString.Append($"|{field.Item1.Field}:{field.Item2.GetValue(serverAction)}");

            return actionString.ToString();
        }
    }
}
