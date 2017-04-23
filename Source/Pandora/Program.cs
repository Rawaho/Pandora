using System;
using System.Diagnostics;
using Pandora.Cryptography;
using Pandora.Database;
using Pandora.Managers;
using Pandora.Network;
using Pandora.Network.Http;

namespace Pandora
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            LogManager.Initialise();
            ConfigManager.Initialise();

            Console.Title = $"Pandora - Server Version: {ConfigManager.Config.Server.Version}";

            CryptoProvider.Initialise();
            DatabaseManager.Initialise();
            AssetManager.Initialise();
            HttpManager.Initialise();
            NetworkManager.Initialise();

            LogManager.Write("Game", $"Server initialised in {sw.ElapsedMilliseconds}ms");
        }
    }
}
