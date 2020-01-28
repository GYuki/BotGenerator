using BotService.API.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BotService.API.Infrastructure.Repositories
{
    public class SqlSubscribeRepository : ISubscribeRepository
    {
        private readonly BotContext _context;

        public SqlSubscribeRepository(BotContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetSubscribersAsync(int botId)
        {
            var chats = await _context.Subscribes
                                .Where(s => s.BotId == botId)
                                .Select(s => s.ChatId)
                                .ToListAsync();

            return chats;
        }

        public async Task<bool> DeleteSubscriptionAsync(string botName, string chatId)
        {
            var subscribe = await _context.Subscribes.SingleOrDefaultAsync(s => s.Bot.Name == botName && s.ChatId == chatId);
            
            if (subscribe is null)
                return false;
            
            _context.Subscribes.Remove(subscribe);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task SubscribeAsync(Subscribe _subscribe)
        {
            var subscribe = new Subscribe
            {
                BotId = _subscribe.BotId,
                ChatId = _subscribe.ChatId
            };

            _context.Subscribes.Add(subscribe);

            await _context.SaveChangesAsync();
        }
    }
}