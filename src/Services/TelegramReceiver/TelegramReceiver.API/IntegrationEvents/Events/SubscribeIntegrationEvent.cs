using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;


namespace TelegramReceiver.API.IntegrationEvents.Events
{
    public class SubscribeIntegrationEvent : IntegrationEvent
    {
        public string BotToken { get; private set; }
        public int ChatId { get; private set; }

        public SubscribeIntegrationEvent(string botToken, int chatId)
        {
            BotToken = botToken;
            ChatId = chatId;
        }
    }
}