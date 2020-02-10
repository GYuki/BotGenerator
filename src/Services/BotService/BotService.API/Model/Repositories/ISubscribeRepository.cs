using System.Threading.Tasks;
using System.Collections.Generic;

namespace BotService.API.Model
{
    public interface ISubscribeRepository
    {
        Task<List<int>> GetSubscribersAsync(int botId);
        Task<List<int>> GetSubscribersAsync(string botToken);
        Task<bool> CreateSubscriptionIfNotExistsAsync(string botToken, int chatId);
        Task<bool> DeleteSubscriptionAsync(string botName, int chatId);
        Task SubscribeAsync(Subscribe subscribe);
    }
}