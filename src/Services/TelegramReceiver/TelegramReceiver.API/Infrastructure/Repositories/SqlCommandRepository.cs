using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TelegramReceiver.API.Models;
using Microsoft.EntityFrameworkCore;


namespace TelegramReceiver.API.Infrastructure.Repositories
{
    public class SqlCommandRepository : ICommandRepository
    {
        private readonly TelegramContext _context;

        public SqlCommandRepository(TelegramContext context)
        {
            _context = context;
        }
        public async Task<Command> GetCommandAsync(int id)
        {
            var command = await _context.Commands
                // .Select(c => new Command
                // {
                //     Id = c.Id,
                //     Request = c.Request,
                //     Response = c.Response
                // })
                .SingleOrDefaultAsync(c => c.Id == id);

            return command;
        }

        public async Task<Command> GetCommandByTokenAndRequestAsync(string token, string request)
        {
            var command = await _context.Commands
                .SingleOrDefaultAsync(c => c.Token == token && c.Request == request);
            
            return command;
        }

        public async Task<List<Command>> GetCommandsByTokenAsync(string token)
        {
            var commands = await _context.Commands
                // .Select(c => new Command
                // {
                //     Id = c.Id,
                //     Request = c.Request,
                //     Response = c.Response
                // })
                .Where(c => c.Token == token)
                .ToListAsync();
            
            return commands;
        }

        public async Task<Command> UpdateCommandAsync(Command _command)
        {
            var command = await _context.Commands.FindAsync(_command.Id);

            if (command is null)
                return command;
            
            command.Response = _command.Response;
            _context.Commands.Update(command);

            await _context.SaveChangesAsync();
            
            return command;
        }

        public async Task<bool> DeleteCommandAsync(int id)
        {
            var command = await _context.Commands.FindAsync(id);

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
                Token = _command.Token,
                Response = _command.Response,
                Description = _command.Description
            };

            _context.Commands.Add(command);

            await _context.SaveChangesAsync();
        }
    }
}