using System;
using System.Data;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Pandora.Cryptography;
using Pandora.Database;

namespace Pandora.Network.Http
{
    public static class AuthHandler
    {
        [HttpPath("/pandora/auth/")]
        public static async void Authentication(string[] parameters, HttpListenerContext context)
        {
            if (parameters.Length != 2)
                return;

            Action<HttpStatusCode, string> sendResponse = (status, payload) =>
            {
                byte[] bytes = Encoding.UTF8.GetBytes(payload);
                context.Response.StatusCode      = (int)status;
                context.Response.ContentLength64 = payload.Length;
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            };

            DataRow accountInfo = await DatabaseManager.DataBase.GetAccountAuth(parameters[0], parameters[1]);
            if (accountInfo == null)
            {
                sendResponse(HttpStatusCode.NotFound, "Not found");
                return;
            }

            string authToken = HashProvider.Sha256(CryptoProvider.Salt(64u));
            await DatabaseManager.DataBase.AccountTokenUpdate(authToken, accountInfo.Read<uint>("id"));

            string accountTokenB64 = JsonConvert.SerializeObject(new AccountToken
            {
                Id       = accountInfo.Read<uint>("id"),
                SteamId  = 0ul,
                Username = accountInfo.Read<string>("username")
            }).ToBase64();

            sendResponse(HttpStatusCode.OK, $"{accountTokenB64}.{authToken}");
        }
    }
}
