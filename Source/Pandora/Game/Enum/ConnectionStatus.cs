using System.Diagnostics.CodeAnalysis;

namespace Pandora.Game.Enum
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum ConnectionStatus
    {
        IN_LOBBY,
        ONLINE,
        INGAME,
        SPECTATING,
        OFFLINE,
        IN_QUEUE
    }
}
