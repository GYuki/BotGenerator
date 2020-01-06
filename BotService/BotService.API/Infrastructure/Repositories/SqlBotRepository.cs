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

        public async Task<Bot> GetBotAsync(string botName)
        {
            var bot = await _context.Bots.SingleOrDefaultAsync(b => b.Name == botName);

            return bot;
        }

        public async Task<List<Bot>> GetBotsOfOwner(int ownerId)
        {
            var bots = await _context.Bots.Where(b => b.OwnerId == ownerId).ToListAsync();

            return bots;
        }

        public async Task<bool> DeleteBotAsync(string botName)
        {
            var bot = await GetBotAsync(botName);

            if (bot is null)
                return false;
            
            _context.Bots.Remove(bot);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateBotAsync(Bot _bot)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == _bot.OwnerId);

            if (user is null)
                return false;
            
            var bot = new Bot
            {
                Owner = user,
                Token = _bot.Token,
                Name = _bot.Name
            };

            _context.Bots.Add(bot);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}