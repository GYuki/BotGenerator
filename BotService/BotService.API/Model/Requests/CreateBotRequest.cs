namespace BotService.API.Model
{
    public class CreateBotRequest
    {
        public string OwnerId { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
    }
}