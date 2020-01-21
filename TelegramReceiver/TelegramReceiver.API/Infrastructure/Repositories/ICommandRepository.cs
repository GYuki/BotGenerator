using System.Threading.Tasks;
using System.Collections.Generic;
using TelegramReceiver.API.Models;


namespace TelegramReceiver.API.Infrastructure.Repositories
{
    public interface ICommandRepository
    {
        Task<Command> GetCommandAsync(int id);
        Task<Command> GetCommandByTokenAndRequestAsync(string botToken, string request);
        Task<List<Command>> GetCommandsByTokenAsync(string botToken);
        Task<Command> UpdateCommandAsync(Command command);
        Task<bool> DeleteCommandAsync(int id);
        Task CreateCommandAsync(Command command);
    }
}