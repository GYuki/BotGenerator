namespace BotService.API.Model
{
    public class Command
    {
        public int Id { get; set; }
        public string Request{ get; set; }
        public string Response { get; set; }
        public Bot Bot { get; set; }
        public int BotId { get; set; }
    }
}