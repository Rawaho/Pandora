using System;
using System.IO;

namespace Pandora.Managers
{
    public class LogManager
    {
        private static readonly object muxtex = new object();

        public static void Initialise()
        {
            File.Delete(@".\Server.log");
        }

        public static void Write(string prefix, string message)
        {
            Console.WriteLine(!string.IsNullOrEmpty(prefix) ? $"[{prefix}] {message}" : message);
            lock (muxtex)
                File.AppendAllText(@".\Server.log", "");
        }
    }
}
