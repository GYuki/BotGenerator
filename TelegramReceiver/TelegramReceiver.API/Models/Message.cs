using Newtonsoft.Json;
using System.Collections.Generic;


namespace TelegramReceiver.API.Models
{
    public class Message
    {
        [JsonProperty("message_id")]
        public int MessageId { get; set; }

        public User From { get; set; }
        public int Date { get; set; }
        public Chat Chat { get; set; }

        [JsonProperty("forward_from")]
        public User ForwardFrom { get; set; }

        public string Text { get; set; }

        public ICollection<MessageEntity> Entities { get; set; }

        [JsonProperty("caption_entities")]
        public ICollection<MessageEntity> CaptionEntities { get; set; }

        public Audio Audio { get; set; }
        public Document Document { get; set; }
        public ICollection<PhotoSize> Photo { get; set; }
        public Video Video { get; set; }
    }
}