using System.Collections.Generic;

namespace BotService.API.Model
{
    public class User
    {
        public int Id { get; set; }
        public string SenderId{ get; set; }

        public ICollection<Bot> Bots { get; set; }
    }
}