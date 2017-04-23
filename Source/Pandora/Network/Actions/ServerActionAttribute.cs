using System;

namespace Pandora.Network.Actions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServerActionAttribute : Attribute
    {
        public string Action { get; }

        public ServerActionAttribute(string action)
        {
            Action = action;
        }
    }
}
