using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using BotService.API.Model;
using BotService.API.IntegrationEvents.Events;
using BotService.API.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;


namespace BotService.API.IntegrationEvents.EventHandling
{
    public class SubscribeIntegrationEventHandler : IIntegrationEventHandler<SubscribeIntegrationEvent>
    {
        private readonly ISubscribeRepository _repository;

        public SubscribeIntegrationEventHandler(ISubscribeRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(SubscribeIntegrationEvent @event)
        {
            var checkSubscription = await _repository.CreateSubscriptionIfNotExistsAsync(@event.BotToken, @event.ChatId);
        }
    }
}