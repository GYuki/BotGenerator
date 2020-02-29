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

        public async Task<List<int>> GetSubscribersAsync(int botId)
        {
            var chats = await _context.Subscribes
                                .Where(s => s.BotId == botId)
                                .Select(s => s.ChatId)
                                .ToListAsync();

            return chats;
        }

        public async Task<List<int>> GetSubscribersAsync(string botToken)
        {
            var chats = await _context.Subscribes
                                .Where(s => s.Bot.Token == botToken)
                                .Select(s => s.ChatId)
                                .ToListAsync();
            
            return chats;
        }

        public async Task<bool> CreateSubscriptionIfNotExistsAsync(string botToken, int chatId)
        {
            var chat = await _context.Subscribes
                               .SingleOrDefaultAsync(s => s.Bot.Token == botToken && s.ChatId == chatId);
            
            if (chat != null)
                return false;
            
            var bot = await _context.Bots.SingleOrDefaultAsync(b => b.Token == botToken);

            if (bot is null)
                return false;

            var subscription = new Subscribe
            {
                BotId = bot.Id,
                ChatId = chatId
            };

            _context.Subscribes.Add(subscription);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteSubscriptionAsync(string botName, int chatId)
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