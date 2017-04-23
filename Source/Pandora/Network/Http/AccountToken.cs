using Newtonsoft.Json;

namespace Pandora.Network.Http
{
    public class AccountToken
    {
        [JsonProperty(PropertyName = "id")]
        public uint Id { get; set; }
        [JsonProperty(PropertyName = "steamId")]
        public ulong SteamId { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
    }
}
