using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace TelegramReceiver.API.Models
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Command
    {
        public int Id { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Token { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Request { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Response { get; set; }
    }
}