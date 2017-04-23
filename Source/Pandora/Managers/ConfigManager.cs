using System.IO;
using Newtonsoft.Json;

namespace Pandora.Managers
{
    public struct ConfigServer
    {
        public string Version { get; set; }
        public string PayloadVersion { get; set; }
    }

    public struct ConfigDatabase
    {
        public string Host { get; set; }
        public uint Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public struct Config
    {
        public ConfigServer Server { get; set; }
        public ConfigDatabase Database { get; set; }
    }

    public class ConfigManager
    {
        public static Config Config { get; private set; }

        public static void Initialise()
        {
            try
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@".\Config.json"));
            }
            catch
            {
                LogManager.Write("Config", "An exception occured while loading the configuration file!");
                throw;
            }
        }
    }
}
