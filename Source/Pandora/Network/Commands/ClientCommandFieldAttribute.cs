using System;

namespace Pandora.Network.Commands
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ClientCommandFieldAttribute : Attribute
    {
        public string Field { get; }

        public ClientCommandFieldAttribute(string field)
        {
            Field = field;
        }
    }
}
