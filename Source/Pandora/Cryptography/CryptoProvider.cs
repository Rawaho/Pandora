using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Pandora.Managers;

namespace Pandora.Cryptography
{
    public static class CryptoProvider
    {
        private static X509Certificate2 rsaCertificate;

        public static void Initialise()
        {
            try
            {
                rsaCertificate = new X509Certificate2(@".\Server.pfx");
            }
            catch
            {
                LogManager.Write("Cryptography", "An exception occured while loading the command private key!");
                throw;
            }
        }

        public static string Decrypt(string cipherKey, string cipherText)
        {
            return AesDecrypt(RsaDecrypt(cipherKey), cipherText);
        }

        private static byte[] RsaDecrypt(string data)
        {
            var certificate = new X509Certificate2(rsaCertificate);
            using (RSACryptoServiceProvider rsaProvider = (RSACryptoServiceProvider)certificate.PrivateKey)
                return rsaProvider.Decrypt(data.ToByteArray(), false);
        }

        private static string AesDecrypt(byte[] key, string data)
        {
            using (var aesProvider = new RijndaelManaged
            {
                Mode      = CipherMode.CBC,
                Padding   = PaddingMode.PKCS7,
                KeySize   = 0x80,
                BlockSize = 0x80,
                IV        = new byte[0x10],
                Key       = key
            })
            {
                byte[] bytes = data.ToByteArray();
                return Encoding.UTF8.GetString(aesProvider.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length));
            }
        }

        public static string Salt(uint length)
        {
            const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            var randomBytes = new byte[length];
            using (var randomProvider = new RNGCryptoServiceProvider())
                randomProvider.GetNonZeroBytes(randomBytes);

            var result = new StringBuilder();
            foreach (byte b in randomBytes)
                result.Append(characters[b % characters.Length]);

            return result.ToString();
        }
    }
}
