using System;

namespace Pandora.Network.Actions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ServerActionFieldAttribute : Attribute
    {
        public string Field { get; }
        public uint Weight { get; }
        public bool SendDefault { get; }

        public ServerActionFieldAttribute(string field, uint weight = uint.MaxValue, bool sendDefault = true)
        {
            Field       = field;
            Weight      = weight;
            SendDefault = sendDefault;
        }
    }
}
