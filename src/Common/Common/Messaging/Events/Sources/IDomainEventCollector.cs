namespace Common.Messaging.Events.Sources
{
    public interface IDomainEventCollector
    {
        void AddEventSource(IHasDomainEvents source);
        IReadOnlyCollection<DomainEvent> GetEvents();
        void ClearEvents();
    }
}
