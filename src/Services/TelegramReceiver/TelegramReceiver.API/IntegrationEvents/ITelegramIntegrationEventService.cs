using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;


namespace TelegramReceiver.API.IntegrationEvents
{
    public interface ITelegramIntegrationEventService
    {
        Task SaveEventAsync(IntegrationEvent evt, System.Guid guid);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}