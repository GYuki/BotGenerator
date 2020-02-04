using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Utilities;
using Microsoft.Extensions.Logging;
using BotService.API;
using BotService.API.Infrastructure;
using Microsoft.EntityFrameworkCore;


namespace BotService.API.IntegrationEvents
{
    public class BotIntegrationEventService
        : IBotIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly BotContext _botContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<BotIntegrationEventService> _logger;

        public BotIntegrationEventService(
            ILogger<BotIntegrationEventService> logger,
            IEventBus eventBus,
            BotContext botContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _botContext = botContext ?? throw new ArgumentNullException(nameof(botContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_botContext.Database.GetDbConnection());
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

        public async Task SaveEventAndBotContextChangesAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- BotIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
            await ResilientTransaction.New(_botContext).ExecuteAsync(async () =>
            {
                // Achieving atomicity between original bot database operation and the IntegrationEventLog thanks to a local transaction
                await _botContext.SaveChangesAsync();
                await _eventLogService.SaveEventAsync(evt, _botContext.Database.CurrentTransaction);
            });
        }
    }
}