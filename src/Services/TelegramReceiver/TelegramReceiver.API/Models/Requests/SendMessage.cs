using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace TelegramReceiver.API.Models
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SendMessage
    {
        public int Id { get; set; }
        [JsonProperty("chat_id")]
        public int ChatId { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Text { get; set; }
    }
}