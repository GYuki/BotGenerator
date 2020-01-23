using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace TelegramReceiver.API.Models
{
     [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class MessageEntity
    {
        [JsonProperty(Required = Required.Always)]
        public string Type { get; set; }
        [JsonProperty(Required = Required.Always)]
        public int Offset { get; set; }
        [JsonProperty(Required = Required.Always)]
        public int Length { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Url {get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public User User { get; set; }
    }
}