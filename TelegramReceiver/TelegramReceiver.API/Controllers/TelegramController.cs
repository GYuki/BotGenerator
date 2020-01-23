using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TelegramReceiver.API.Models;
using TelegramReceiver.API.Infrastructure.Repositories;
using Newtonsoft.Json;

namespace TelegramReceiver.API.Controllers
{
    [Produces("application/json")]
    [Route("receiver/[controller]")]
    [ApiController]
    public class TelegramController : Controller
    {
        private readonly ICommandRepository _commandRepository;
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://api.telegram.org/bot";

        public TelegramController(ICommandRepository commandRepository)
        {
            _commandRepository = commandRepository;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        [HttpPost]
        [Route("{botToken}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> HandleMessageAsync(string botToken, [FromBody]Update update)
        {
            var myMessage = new SendMessage()
            {
                ChatId = update.Message.Chat.Id,
                Text = update.Message.Text
            };
            await SendMessageToBotAsync(botToken, myMessage);
            return Ok();
        }

        private async Task SendMessageToBotAsync(
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
    }
}