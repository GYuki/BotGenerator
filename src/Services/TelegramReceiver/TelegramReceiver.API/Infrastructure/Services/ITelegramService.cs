using System.Threading;
using System.Threading.Tasks;
using TelegramReceiver.API.Models;


namespace TelegramReceiver.API.Infrastructure.Services
{
    public interface ITelegramService
    {
        Task SendMessageToBotAsync(string botToken, SendMessage sendMessage, CancellationToken token = default);
        Task<string> GenerateResponseTextAsync(string command, string botToken, int chatId);
    }
}