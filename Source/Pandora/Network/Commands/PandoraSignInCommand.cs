using System;
using Newtonsoft.Json;
using Pandora.Cryptography;
using Pandora.Database;
using Pandora.Game;
using Pandora.Managers;
using Pandora.Network.Actions;
using Pandora.Network.Http;

namespace Pandora.Network.Commands
{
    // custom packet
    [Serializable]
    [ClientCommand("PandoraSignIn", ServerType.WorldServer)]
    public class PandoraSignInCommand : ClientCommand
    {
        [ClientCommandField("payloadVersion")]
        public string PayloadVersion { get; set; }
        [ClientCommandField("clientVersion")]
        public string ClientVersion { get; set; }
        [ClientCommandField("token")]
        public string Token { get; set; }

        public override async void Handle(Session session)
        {
            if (PayloadVersion != ConfigManager.Config.Server.PayloadVersion
                || ClientVersion != ConfigManager.Config.Server.Version)
            {
                session.Send(new SignInFailAction(0, "BAD_CLIENT_VERSION"));
                return;
            }

            string[] tokenExplode = Token.Split('.');
            AccountToken accountToken;
            try
            {
                accountToken = JsonConvert.DeserializeObject<AccountToken>(tokenExplode[0].FromBase64());
            }
            catch
            {
                return;
            }

            Account account = await DatabaseManager.DataBase.GetAccount(accountToken.Username, tokenExplode[1]);
            if (account == null)
            {
                session.Send(new SignInFailAction(accountToken.Id, "TOKEN_INVALID"));
                return;
            }

            session.SetAccount(account);
            session.Account.SendSignIn();
        }
    }
}
