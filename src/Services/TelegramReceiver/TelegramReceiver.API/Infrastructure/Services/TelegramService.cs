using System;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TelegramReceiver.API.Models;
using TelegramReceiver.API.Infrastructure.Repositories;
using TelegramReceiver.API.IntegrationEvents;
using TelegramReceiver.API.IntegrationEvents.Events;
using Newtonsoft.Json;


namespace TelegramReceiver.API.Infrastructure.Services
{
    public class TelegramService
        : ITelegramService
    {
        private readonly HttpClient _httpClient;
        private readonly ICommandRepository _commandRepository;
        private readonly ITelegramIntegrationEventService _telegramIntegrationEventService;
        private const string BASE_URL = "https://api.telegram.org/bot";
        

        public TelegramService(ICommandRepository commandRepository, ITelegramIntegrationEventService telegramIntegrationEventService)
        {
            _commandRepository = commandRepository;
            _telegramIntegrationEventService = telegramIntegrationEventService;

            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }
        public async Task SendMessageToBotAsync(
            string botToken,
            SendMessage message,
            CancellationToken cancellationToken = default)
        {
            string url = $"{BASE_URL}{botToken}/sendMessage";
            string payload = await Task.Run(() => JsonConvert.SerializeObject(message));
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            }
            catch (TaskCanceledException e)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw;
                
                throw new TimeoutException("Request timeout", e);
            }
        }

        public async Task<string> GenerateResponseTextAsync(string command, string botToken, int chatId)
        {
            string result = "";
            if (command == "/help")
            {
                StringBuilder sb = new StringBuilder();
                var botCommands = await _commandRepository.GetCommandsByTokenAsync(botToken);

                foreach (Command cmd in botCommands)
                    sb.Append($"{cmd.Request} - {cmd.Description}\n");
                result = sb.ToString();
            }
            else if (command == "/start")
            {
                var subscribeEvent = new SubscribeIntegrationEvent(botToken, chatId);

                await _telegramIntegrationEventService.SaveEventAsync(subscribeEvent, Guid.NewGuid());

                await _telegramIntegrationEventService.PublishThroughEventBusAsync(subscribeEvent);

                result = "Hello!";
            }
            else
            {
                var botCommand = await _commandRepository.GetCommandByTokenAndRequestAsync(botToken, command);
                if (botCommand != null)
                    result = botCommand.Response;
            }
            return result;
        }
    }
}