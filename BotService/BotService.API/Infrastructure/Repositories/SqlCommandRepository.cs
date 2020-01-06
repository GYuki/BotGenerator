using BotService.API.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace BotService.API.Infrastructure.Repositories
{
    public class SqlCommandRepository : ICommandRepository
    {
        private readonly BotContext _context;

        public SqlCommandRepository(BotContext context)
        {
            _context = context;
        }

        public async Task<List<Command>> GetBotCommandsAsync(int botId)
        {
            var commands = await _context.Commands.Where(c => c.BotId == botId).ToListAsync();

            return commands;
        }

        public async Task<Command> UpdateCommandResponseAsync(Command _command)
        {
            var command = await _context.Commands.SingleOrDefaultAsync(u => u.Id == _command.Id);

            command.Response = _command.Response;
            _context.Commands.Update(command);

            await _context.SaveChangesAsync();

            return command;
        }

        public async Task<bool> DeleteCommandAsync(string commandName, int botId)
        {
            var command = await _context.Commands.SingleOrDefaultAsync(c => c.BotId == botId);

            if (command is null)
                return false;
            
            _context.Commands.Remove(command);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task CreateCommandAsync(Command _command)
        {
            var command = new Command
            {
                Request = _command.Request,
                Response = _command.Response,
                BotId = _command.BotId
            };

            _context.Commands.Add(command);

            await _context.SaveChangesAsync();
        }
    }
}