namespace Common.Messaging.Events.Sources
{
    public interface IDomainEventBuffer
    {
        void AddEventSource(IHasDomainEvents source);
        IReadOnlyCollection<DomainEvent> GetEvents();
        void ClearEvents();
    }
}
