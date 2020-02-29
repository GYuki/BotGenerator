namespace BotService.API.Model
{
    public class UpdateCommandRequest
    {
        public bool HasNullField
        {
            get => string.IsNullOrEmpty(Request) ||
                   string.IsNullOrEmpty(Response) ||
                   string.IsNullOrEmpty(BotName);
        }
        public string Request { get; set; }
        public string Response { get; set; }
        public string BotName { get; set; }
    }
}