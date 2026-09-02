using Common.Messaging.Events;
using Common.Messaging.Events.Services;
using EasyNetQ;

namespace Common.Messaging
{
    public class EasyNetQPublisher(IBus Bus) : IEventPublisher
    {
        public async Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            await PublishAsync((dynamic)integrationEvent, cancellationToken);
        }

        private async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IntegrationEvent
        {
            await Bus.PubSub.PublishAsync(integrationEvent, cancellationToken);
        }
    }
}
