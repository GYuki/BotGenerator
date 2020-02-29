using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TelegramReceiver.API.Infrastructure;


namespace TelegramReceiver.API.IntegrationEvents
{
    public class TelegramIntegrationEventService
        : ITelegramIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly TelegramContext _telegramContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<TelegramIntegrationEventService> _logger;

        public TelegramIntegrationEventService(
            ILogger<TelegramIntegrationEventService> logger,
            IEventBus eventBus,
            TelegramContext telegramContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _telegramContext = telegramContext ?? throw new ArgumentNullException(nameof(telegramContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_telegramContext.Database.GetDbConnection());
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                _eventBus.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task SaveEventAsync(IntegrationEvent evt, System.Guid guid)
        {
            _logger.LogInformation("----- BotIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

            await _eventLogService.SaveEventAsync(evt, guid);
        }
    }
}