using System.Threading.Tasks;
using System.Collections.Generic;

namespace BotService.API.Model
{
    public interface IBotRepository
    {
        Task<Bot> GetBotAsync(string botName);
        Task<List<Bot>> GetBotsOfOwner(int ownerId);
        Task<bool> DeleteBotAsync(string botName);
        Task<bool> CreateBotAsync(Bot bot);
    }
}