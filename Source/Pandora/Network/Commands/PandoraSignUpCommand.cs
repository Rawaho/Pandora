using System;
using Pandora.Database;
using Pandora.Managers;
using Pandora.Network.Actions;

namespace Pandora.Network.Commands
{
    // custom packet
    [Serializable]
    [ClientCommand("PandoraSignUp", ServerType.WorldServer)]
    public class PandoraSignUpCommand : ClientCommand
    {
        [ClientCommandField("payloadVersion")]
        public string PayloadVersion { get; set; }
        [ClientCommandField("clientVersion")]
        public string ClientVersion { get; set; }
        [ClientCommandField("username")]
        public string Username { get; set; }
        [ClientCommandField("password")]
        public string Password { get; set; }

        public override async void Handle(Session session)
        {
            if (PayloadVersion != ConfigManager.Config.Server.PayloadVersion
                || ClientVersion != ConfigManager.Config.Server.Version)
            {
                session.Send(new SignUpFailAction("BAD_CLIENT_VERSION"));
                return;
            }

            int result = await DatabaseManager.DataBase.CreateAccount(Username, Password);
            if (result == -1)
            {
                session.Send(new SignUpFailAction("STEAM_ACCOUNT_ALREADY_BOUND"));
                return;
            }

            session.SetAccount(await DatabaseManager.DataBase.GetAccount(Username, ""));
            session.Account.SendSignIn();
        }
    }
}
