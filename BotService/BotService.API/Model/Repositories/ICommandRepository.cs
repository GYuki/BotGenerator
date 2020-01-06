using System.Threading.Tasks;
using System.Collections.Generic;

namespace BotService.API.Model
{
    public interface ICommandRepository
    {
        Task<List<Command>> GetBotCommandsAsync(int botId);
        Task<Command> UpdateCommandResponseAsync(Command command);
        Task<bool> DeleteCommandAsync(string commandName, int botId);
        Task CreateCommandAsync(Command command);
    }
}