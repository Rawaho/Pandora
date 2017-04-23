using System;
using System.Security.Cryptography;
using System.Text;

namespace Pandora.Cryptography
{
    public static class HashProvider
    {
        public static string Sha1(string data)
        {
            byte[] bytes = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public static string Sha256(string data)
        {
            byte[] bytes = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
