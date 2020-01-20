using System.Threading.Tasks;
using System.Collections.Generic;

namespace BotService.API.Model
{
    public interface ICommandRepository
    {
        Task<Command> GetCommandAsync(int id);
        Task<List<Command>> GetBotCommandsAsync(int botId);
        Task<Command> UpdateCommandResponseAsync(Command command);
        Task<bool> DeleteCommandAsync(int commandId);
        Task CreateCommandAsync(Command command);
    }
}