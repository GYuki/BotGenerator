using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;


namespace TelegramReceiver.API.Models
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Message
    {
        [JsonProperty(Required = Required.Always)]
        public int MessageId { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public User From { get; set; }
        public int Date { get; set; }
        [JsonProperty(Required = Required.Always)]
        public Chat Chat { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public User ForwardFrom { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ICollection<MessageEntity> Entities { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ICollection<MessageEntity> CaptionEntities { get; set; }
    }
}