using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using models = global::BotService.API.Model;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GrpcBot
{
    public class BotService : Bot.BotBase
    {
        private readonly models.IBotRepository _repository;
        private readonly ILogger<BotService> _logger;
        
        public BotService(models.IBotRepository repository, ILogger<BotService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [AllowAnonymous]
        public override async Task<BotResponse> GetBotById(BotRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call from method {Method} for bot id {Id}", context.Method, request.Id);

            var data = await _repository.GetBotAsync(request.Id);

            if (data != null)
            {
                context.Status = new Status(StatusCode.OK, $"Bot with ID {request.Id} do exist");

                return MapToBotResponse(data);
            }
            else
            {
                context.Status = new Status(StatusCode.NotFound, $"Bot with id {request.Id} do not exist");
            }

            return new BotResponse();
        }

        public override async Task<Empty> CreateBot(MakeBotRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call BotService.CreateBot. Bot token {Token}", request.Bot.Token);

            await _repository.CreateBotAsync(MapToBotModel(request));

            return new Empty();
        }

        private BotResponse MapToBotResponse(models.Bot bot)
        {
            return new BotResponse()
            {
                Id = bot.Id,
                Name = bot.Name,
                Token = bot.Token
            };
        }

        private models.Bot MapToBotModel(MakeBotRequest request)
        {
            return new models.Bot
            {
                Id = request.Bot.Id,
                Name = request.Bot.Name,
                Token = request.Bot.Token
            };
        }
    }
}