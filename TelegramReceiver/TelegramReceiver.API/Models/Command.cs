namespace TelegramReceiver.API.Models
{
    public class Command
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Request { get; set; }
        public Message Response { get; set; }
    }
}