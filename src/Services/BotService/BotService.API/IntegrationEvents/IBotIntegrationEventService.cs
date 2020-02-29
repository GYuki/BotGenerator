using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;


namespace BotService.API.IntegrationEvents
{
    public interface IBotIntegrationEventService
    {
        Task SaveEventAndBotContextChangesAsync(IntegrationEvent evt, System.Guid guid);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}