using System.Data;
using Pandora.Database;
using Pandora.Game.Enum;
using Pandora.Network;
using Pandora.Network.Actions;
using Pandora.Network.Actions.Data;

namespace Pandora.Game
{
    public class Account
    {
        public uint Id { get; }
        public string Username { get; }
        public uint Level { get; }
        public uint Xp { get; }
        public uint Coins { get; }
        public uint Gems { get; }
        public uint Flags { get; }

        public Session Session { get; set; }

        public Account(DataRow data)
        {
            Id       = data.Read<uint>("id");
            Username = data.Read<string>("username");
            Level    = data.Read<uint>("level");
            Xp       = data.Read<uint>("xp");
            Coins    = data.Read<uint>("coins");
            Gems     = data.Read<uint>("gems");
            Flags    = data.Read<uint>("flags");
        }

        public void SendSignIn()
        {
            var transaction = new ServerActionTransaction();
            transaction.Actions.Add(new AccountData
            {
                Id                 = Id,
                UserName           = Username,
                Level              = Level,
                LevelXp            = Xp,
                NextLevelXp        = 280u,
                Coins              = Coins,
                Gems               = Gems,
                ClientVersionValid = true,
                Status             = AccountStatus.VALIDATED,
            });

            // TODO: implement this
            // need to send account chest information otherwise client exception
            transaction.Actions.Add(new AccountChestData
            {
                Id = 0,
                ChestType = ChestType.BLUE,
                Count = 0,
                OpenedCount = 0
            });

            transaction.Actions.Add(new AccountChestData
            {
                Id = 1,
                ChestType = ChestType.ORANGE,
                Count = 0,
                OpenedCount = 0
            });

            transaction.Actions.Add(new AccountChestData
            {
                Id = 2,
                ChestType = ChestType.PINK,
                Count = 0,
                OpenedCount = 0
            });

            // TODO: implement this
            // need to send to move the client to the main menu and not the tutorial screen
            transaction.Actions.Add(new CompletedQuestData
            {
                Id = 5,
                CompletionDate = 0
            });

            transaction.Actions.Add(new SignInSuccessAction());
            Session.Send(transaction);
        }
    }
}
