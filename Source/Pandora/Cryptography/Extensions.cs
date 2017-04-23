using System;
using System.Linq;
using System.Text;

namespace Pandora.Cryptography
{
    public static class Extensions
    {
        public static byte[] ToByteArray(this string hexString)
        {
            return Enumerable.Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                .ToArray();
        }

        public static string ToBase64(this string data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }

        public static string FromBase64(this string data)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(data));
        }
    }
}
