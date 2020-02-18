using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using models = global::TelegramReceiver.API.Models;
using TelegramReceiver.API.Infrastructure.Services;

namespace GrpcTelegram
{
    public class TelegramService : Telegram.TelegramBase
    {
        private readonly ITelegramService _service;
        private readonly ILogger<TelegramService> _logger;

        public TelegramService(ITelegramService service , ILogger<TelegramService> logger)
        {
            _service = service;
            _logger = logger;
        }

        public override async Task<MessageResponse> HandleMessage(MessageRequest messageRequest, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call TelegramService.HandleMessage");
            string msg = "";
            int responseCode = 200;

            var update = messageRequest.Update;
            
            var sendMessageObject = new models.SendMessage
            {
                ChatId = update.Message.Chat.Id,
            };

            if (update.Message.Entities == null ||
                update.Message.Entities.Count != 1 ||
                update.Message.Entities[0].Type != "bot_command")
            {
                sendMessageObject.Text = "ERROR. I can only handle single bot command.";
                responseCode = (int)System.Net.HttpStatusCode.Accepted;
            }
            else
            {
                var command = update.Message.Text.Substring(
                    update.Message.Entities[0].Offset,
                    update.Message.Entities[0].Length
                );
                msg = await _service.GenerateResponseTextAsync(command, messageRequest.BotToken, update.Message.Chat.Id);

                if (string.IsNullOrEmpty(msg))
                    msg = "No commands found";

                sendMessageObject.Text = msg;
                responseCode = (int)System.Net.HttpStatusCode.OK;
            }

            await _service.SendMessageToBotAsync(messageRequest.BotToken, sendMessageObject);
            return new MessageResponse
            {
                ResponseCode = responseCode
            };
        }
    }
}