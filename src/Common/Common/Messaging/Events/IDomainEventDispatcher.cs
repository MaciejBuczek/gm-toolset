namespace Common.Messaging.Events
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventsAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken = default);
    }
}
