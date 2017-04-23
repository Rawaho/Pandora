namespace Pandora.Network.Commands
{
    [ClientCommand("$a", ServerType.WorldServer | ServerType.GameServer)]
    public class KeepAliveCommand : ClientCommand
    {
        [ClientCommandField("cc")]
        public uint ClickCount { get; set; }
        [ClientCommandField("afk")]
        public uint Afk { get; set; }
        [ClientCommandField("s")]
        public long LastSequence { get; set; }

        public override void Handle(Session session)
        {
            // TODO
        }
    }
}
