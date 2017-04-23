using System;

namespace Pandora.Network
{
    [Flags]
    public enum ServerType
    {
        None        = 0x00,
        WorldServer = 0x01,
        GameServer  = 0x02
    }
}
