namespace Common.Messaging.Events.Services
{
    public class IntegrationEventDispatcher(IEventPublisher EventPublisher) : IEventDispatcher<IntegrationEvent>
    {
        public async Task DispatchEventsAsync(IEnumerable<IntegrationEvent> events, CancellationToken cancellationToken = default)
        {
            foreach (var integrationEvent in events)
            {
                await EventPublisher.PublishAsync(integrationEvent, cancellationToken);
            }
        }
    }
}
