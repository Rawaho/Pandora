using System;
using Pandora.Game.Enum;

namespace Pandora.Network.Actions.Data
{
    [Serializable]
    public class AccountData : DataSSetAction
    {
        [ServerActionField("annuityDueDays")]
        public uint AnnuityDueDays { get; set; }
        [ServerActionField("arenaPoints")]
        public uint ArenaPoints { get; set; }
        [ServerActionField("arenaTicketFragmentCount")]
        public uint ArenaTicketFragmentCount { get; set; }
        [ServerActionField("canRerollDailyQuests")]
        public bool CanRerollDailyQuests { get; set; }
        [ServerActionField("canSetRecruiter")]
        public bool CanSetRecruiter { get; set; }
        [ServerActionField("clientVersionValid")]
        public bool ClientVersionValid { get; set; }
        [ServerActionField("coins")]
        public uint Coins { get; set; }
        [ServerActionField("connectionStatus")]
        public ConnectionStatus ConnectionStatus { get; set; }
        [ServerActionField("constructedGodRank")]
        public uint ConstructedGodRank { get; set; }
        [ServerActionField("constructedRank")]
        public uint ConstructedRank { get; set; }
        [ServerActionField("constructedStars")]
        public uint ConstructedStars { get; set; }
        [ServerActionField("dailyRewardGridId")]
        public string DailyRewardGridId { get; set; }
        [ServerActionField("dailyRewardGridPosition")]
        public uint DailyRewardGridPosition { get; set; }
        [ServerActionField("dusts")]
        public uint Dusts { get; set; }
        [ServerActionField("email")]
        public string Email { get; set; }
        [ServerActionField("gems")]
        public uint Gems { get; set; }
        [ServerActionField("level")]
        public uint Level { get; set; }
        [ServerActionField("levelXp")]
        public uint LevelXp { get; set; }
        [ServerActionField("marketCurrency")]
        public MarketCurrency MarketCurrency { get; set; }
        [ServerActionField("moderationLevel")]
        public uint ModerationLevel { get; set; }
        [ServerActionField("nextLevelXp")]
        public uint NextLevelXp { get; set; }
        [ServerActionField("oneMonthAnnuityDaysLeft")]
        public uint OneMonthAnnuityDaysLeft { get; set; }
        [ServerActionField("phantomArenaTicketCount")]
        public uint PhantomArenaTicketCount { get; set; }
        [ServerActionField("pickedAvatarId")]
        public uint PickedAvatarId { get; set; }
        [ServerActionField("pickedCardBackId")]
        public uint PickedCardBackId { get; set; }
        [ServerActionField("pickedDeckId")]
        public long PickedDeckId { get; set; }
        [ServerActionField("pickedOrbId")]
        public uint PickedOrbId { get; set; }
        [ServerActionField("pickedTournamentDeckId")]
        public long PickedTournamentDeckId { get; set; }
        [ServerActionField("pickedWellId")]
        public uint PickedWellId { get; set; }
        [ServerActionField("refuseFriendRequests")]
        public bool RefuseFriendRequests { get; set; }
        [ServerActionField("refuseFriendSpectate")]
        public bool RefuseFriendSpectate { get; set; }
        [ServerActionField("status")]
        public AccountStatus Status { get; set; }
        [ServerActionField("tournamentId")]
        public uint TournamentId { get; set; }
        [ServerActionField("userName")]
        public string UserName { get; set; }

        public AccountData() : base("you", "ACCOUNT") { }
    }
}
