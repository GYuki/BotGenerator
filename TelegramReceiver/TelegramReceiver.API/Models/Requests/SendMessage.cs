using Newtonsoft.Json;


namespace TelegramReceiver.API.Models
{
    public class SendMessage
    {
        public int Id { get; set; }
        [JsonProperty("chat_id")]
        public int ChatId { get; set; }
        public string Text { get; set; }
    }
}