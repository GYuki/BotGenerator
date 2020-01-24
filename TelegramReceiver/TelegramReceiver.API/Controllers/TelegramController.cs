using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TelegramReceiver.API.Models;
using TelegramReceiver.API.Infrastructure.Services;
using Newtonsoft.Json;

namespace TelegramReceiver.API.Controllers
{
    [Produces("application/json")]
    [Route("receiver/[controller]")]
    [ApiController]
    public class TelegramController : Controller
    {
        private readonly ITelegramService _telegramService;
        public TelegramController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        [HttpPost]
        [Route("{botToken}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<ActionResult> HandleMessageAsync(string botToken, [FromBody]Update update)
        {
            ObjectResult resultCode = null;
            string msg = "";
            var messageObject = new SendMessage()
            {
                ChatId = update.Message.Chat.Id,
            };
            
            if (update.Message.Entities == null ||
                update.Message.Entities.Length != 1 || 
                update.Message.Entities[0].Type != "bot_command")
            {
                messageObject.Text = "ERROR. I can only handle single bot command.";
                resultCode = Accepted();
            }
            else
            {
                var command = update.Message.Text.Substring(
                    update.Message.Entities[0].Offset,
                    update.Message.Entities[0].Length
                );
                msg = await _telegramService.GenerateResponseTextAsync(command, botToken);

                if (string.IsNullOrEmpty(msg))
                    msg = "No commands found";

                messageObject.Text = msg;
            }
            
            await _telegramService.SendMessageToBotAsync(botToken, messageObject);
            return Ok();
        }
    }
}