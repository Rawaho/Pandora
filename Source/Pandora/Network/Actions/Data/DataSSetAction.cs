using System;

namespace Pandora.Network.Actions.Data
{
    [Serializable]
    [ServerAction("$sset")]
    public abstract class DataSSetAction : ServerAction
    {
        [ServerActionField("dataRoom", 0u)]
        public string DataRoom { get; }
        [ServerActionField("type", 1u)]
        public string Type { get; }
        [ServerActionField("id", 2u)]
        public uint Id { get; set; }

        protected DataSSetAction(string dataRoom, string type)
        {
            DataRoom = dataRoom;
            Type     = type;
        }
    }
}
