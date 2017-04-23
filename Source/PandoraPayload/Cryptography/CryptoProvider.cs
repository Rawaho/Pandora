using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Abrakam;

namespace PandoraPayload.Cryptography
{
    public class CryptoProvider
    {
        public static void SetPublicKey()
        {
            if (!File.Exists("Client.pfx"))
                return;

            var certificate = new X509Certificate2("Client.pfx");
            ApplicationManager.worldNetworkManager.SetPrivateField<NetworkManager>("rsaEncrypter", (RSACryptoServiceProvider)certificate.PublicKey.Key);
        }
    }
}
