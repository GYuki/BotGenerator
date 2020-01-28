using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace TelegramReceiver.API.Models
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Chat
    {
        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Type { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Title { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Username { get; set; }
    }
}