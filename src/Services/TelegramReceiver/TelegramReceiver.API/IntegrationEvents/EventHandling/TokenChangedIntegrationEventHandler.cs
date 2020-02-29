using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using TelegramReceiver.API.Models;
using TelegramReceiver.API.IntegrationEvents.Events;
using TelegramReceiver.API.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;


namespace TelegramReceiver.API.IntegrationEvents.EventHandling
{
    public class TokenChangedIntegrationEventHandler : IIntegrationEventHandler<TokenChangedIntegrationEvent>
    {
        private readonly ICommandRepository _repository;

        public TokenChangedIntegrationEventHandler(
            ICommandRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(TokenChangedIntegrationEvent @event)
        {
            var commands = await _repository.GetCommandsByTokenAsync(@event.OldToken);

            foreach (var command in commands)
            {
                await UpdateTokenInCommandAsync(@event.OldToken, @event.NewToken, command);
            }
        }

        private async Task UpdateTokenInCommandAsync(string oldToken, string newToken, Command command)
        {
            if (command.Token == oldToken)
                command.Token = newToken;

            await _repository.UpdateCommandAsync(command);
        }
    }
}