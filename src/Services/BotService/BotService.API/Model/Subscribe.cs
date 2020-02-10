namespace BotService.API.Model
{
    public class Subscribe
    {
        public int Id { get; set; }
        public Bot Bot { get; set; }
        public int BotId { get; set; }
        public int ChatId { get; set; }

    }
}