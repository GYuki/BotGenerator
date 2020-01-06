using System.Threading.Tasks;
using System.Collections.Generic;

namespace BotService.API.Model
{
    public interface ISubscribeRepository
    {
        Task<List<string>> GetSubscribersAsync(int botId);
        Task<bool> DeleteSubscriptionAsync(string botName, string chatId);
        Task SubscribeAsync(Subscribe subscribe);
    }
}