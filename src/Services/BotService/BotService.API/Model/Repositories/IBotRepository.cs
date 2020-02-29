using System.Threading.Tasks;
using System.Collections.Generic;

namespace BotService.API.Model
{
    public interface IBotRepository
    {
        Task<Bot> GetBotAsync(int botId);
        Task<Bot> GetBotByTokenAsync(string name);
        Task UpdateBotTokenAsync(Bot currentBot);
        Task<bool> DeleteBotAsync(int botId);
        Task CreateBotAsync(Bot bot);
    }
}