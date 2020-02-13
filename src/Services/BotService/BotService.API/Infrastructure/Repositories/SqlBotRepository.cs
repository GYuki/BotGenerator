using BotService.API.Model;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BotService.API.Infrastructure.Repositories
{
    public class SqlBotRepository : IBotRepository
    {
        private readonly BotContext _context;

        public SqlBotRepository(BotContext context)
        {
            _context = context;
        }

        public async Task<Bot> GetBotAsync(int botId)
        {
            var bot = await _context.Bots
                .SingleOrDefaultAsync(b => b.Id == botId);

            return bot;
        }

        public async Task<Bot> GetBotByTokenAsync(string botToken)
        {
            return await _context.Bots.SingleOrDefaultAsync(x => x.Token == botToken);

        }

        public async Task<List<Bot>> GetBotsOfOwnerAsync(string ownerId)
        {
            var bots = await _context.Bots.Where(b => b.OwnerId == ownerId)
                .ToListAsync();

            return bots;
        }

        public async Task UpdateBotTokenAsync(Bot currentBot)
        {
            _context.Update(currentBot);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteBotAsync(int botId)
        {
            var bot = await GetBotAsync(botId);

            if (bot is null)
                return false;
            
            _context.Bots.Remove(bot);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task CreateBotAsync(Bot _bot)
        {
            var bot = new Bot
            {
                OwnerId = _bot.OwnerId,
                Token = _bot.Token,
                Name = _bot.Name
            };

            _context.Bots.Add(bot);

            await _context.SaveChangesAsync();
        }
    }
}