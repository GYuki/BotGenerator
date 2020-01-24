namespace TelegramReceiver.API.Models
{
    public class Command
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Request { get; set; }
        public int ResponseId { get; set; }
        public string Description { get; set; }
        public string Response { get; set; }
    }
}