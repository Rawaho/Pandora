using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Pandora.Game;
using Pandora.Network.Actions;

namespace Pandora.Network
{
    public class Session
    {
        public Account Account { get; private set; }
        public string IpAddress { get; }

        private uint sequence;
        
        private readonly ServerType type;
        private readonly Socket socket;
        private readonly byte[] buffer = new byte[8192];

        public Session(Socket clientSocket, ServerType serverType)
        {
            socket    = clientSocket;
            type      = serverType;
            IpAddress = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();

            Send(new WelcomeAction
            {
                Source       = type,
                InstanceMode = InstanceMode.debug,
                Nonce        = 0L
            });

            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceive, null);
        }

        public void SetAccount(Account account)
        {
            Account = account;
            Account.Session = this;
        }

        private void OnReceive(IAsyncResult result)
        {
            int dataLength = socket.EndReceive(result);
            if (dataLength <= 0)
                return;

            string packet = Encoding.UTF8.GetString(buffer, 0, dataLength);
            NetworkManager.MarshalPacket(this, packet);

            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceive, null);
        }

        public void Send(ServerActionTransaction transaction)
        {
            var sb = new StringBuilder();
            foreach (ServerAction action in transaction.Actions)
                sb.Append($"{sequence}|{NetworkManager.DeMarshal(action)}\n");

            Send(sb.ToString());
            sequence++;
        }

        public void Send(ServerAction action, bool incrementSequence = true)
        {
            Send($"{sequence}|{NetworkManager.DeMarshal(action)}\n");

            if (incrementSequence)
                sequence++;
        }

        public void Send(string data)
        {
            for (int i = 0; i < data.Length; i += 1480)
            {
                string dataChunk = data.Substring(i, Math.Min(1480, data.Length - i));
                byte[] outBuffer = Encoding.UTF8.GetBytes(dataChunk);
                socket.Send(outBuffer, 0, outBuffer.Length, SocketFlags.None);
            }
        }
    }
}
