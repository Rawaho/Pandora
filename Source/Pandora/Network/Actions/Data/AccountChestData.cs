using System;
using Pandora.Game.Enum;

namespace Pandora.Network.Actions.Data
{
    [Serializable]
    public class AccountChestData : DataSSetAction
    {
        [ServerActionField("chestType")]
        public ChestType ChestType { get; set; }
        [ServerActionField("count")]
        public uint Count { get; set; }
        [ServerActionField("openedCount")]
        public uint OpenedCount { get; set; }

        public AccountChestData() : base("chests", "CHEST") { }
    }
}
