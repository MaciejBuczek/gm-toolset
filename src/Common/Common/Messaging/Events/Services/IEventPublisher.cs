namespace Common.Messaging.Events.Services
{
    public interface IEventPublisher
    {
        Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
    }
}
