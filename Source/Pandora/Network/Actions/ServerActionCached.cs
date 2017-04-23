using System;
using System.Collections.Generic;
using System.Reflection;

namespace Pandora.Network.Actions
{
    public struct ServerActionCached
    {
        public string Action { get; }
        public List<Tuple<ServerActionFieldAttribute, PropertyInfo>> Fields { get; }

        public ServerActionCached(string action)
        {
            Action = action;
            Fields = new List<Tuple<ServerActionFieldAttribute, PropertyInfo>>();
        }
    }
}