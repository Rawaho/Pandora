using System;

namespace Pandora.Network.Actions
{
    [Serializable]
    [ServerAction("$welcome")]
    public class WelcomeAction : ServerAction
    {
        [ServerActionField("source")]
        public ServerType Source { get; set; }
        [ServerActionField("instanceMode")]
        public InstanceMode InstanceMode { get; set; }
        [ServerActionField("nonce")]
        public long Nonce { get; set; }
    }
}
