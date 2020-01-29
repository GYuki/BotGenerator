using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;


namespace TelegramReceiver.API.IntegrationEvents.Events
{
    public class TokenChangedIntegrationEvent : IntegrationEvent
    {
        public string NewToken { get; private set; }
        public string OldToken { get; private set; }

        public TokenChangedIntegrationEvent(string newToken, string oldToken)
        {
            NewToken = newToken;
            OldToken = oldToken;
        }
    }
}