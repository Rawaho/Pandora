using System.Diagnostics.CodeAnalysis;

namespace Pandora.Game.Enum
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum AccountStatus
    {
        GUEST,     // account creation dialog on login
        REGULAR,   // email validation dialog on login
        VALIDATED
    }
}
