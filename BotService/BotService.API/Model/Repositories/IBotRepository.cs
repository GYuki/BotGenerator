using System.Threading.Tasks;
using System.Collections.Generic;

namespace BotService.API.Model
{
    public interface IBotRepository
    {
        Task<Bot> GetBotAsync(int botId);
        Task<List<Bot>> GetBotsOfOwnerAsync(int ownerId);
        Task<bool> DeleteBotAsync(int botId);
        Task CreateBotAsync(Bot bot);
    }
}