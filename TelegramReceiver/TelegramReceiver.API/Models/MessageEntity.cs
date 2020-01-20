using Newtonsoft.Json;


namespace TelegramReceiver.API.Models
{
    public class MessageEntity
    {
        public string Type { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
        public string Url {get; set; }
        public User User { get; set; }
    }
}