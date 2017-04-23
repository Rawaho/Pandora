using System;
using System.Collections.Generic;
using System.Reflection;

namespace Pandora.Network.Commands
{
    public struct ClientCommandCached
    {
        public Type Command { get; }
        public Dictionary<string, PropertyInfo> Fields { get; }

        public ClientCommandCached(Type command)
        {
            Command = command;
            Fields  = new Dictionary<string, PropertyInfo>();
        }
    }
}