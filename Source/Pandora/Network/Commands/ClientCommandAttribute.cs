using System;

namespace Pandora.Network.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ClientCommandAttribute : Attribute
    {
        public string Command { get; }
        public ServerType ServerType { get; }

        public ClientCommandAttribute(string command, ServerType serverType)
        {
            Command    = command;
            ServerType = serverType;
        }
    }
}
