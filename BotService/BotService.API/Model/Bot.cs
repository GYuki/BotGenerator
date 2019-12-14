using System.Collections.Generic;

namespace BotService.API.Model
{
    public class Bot
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public User Owner { get; set; }

        public ICollection<Command> Commands { get; set; }

    }
}