namespace Common.Messaging.Events.Sources
{
    public interface IHasDomainEvents
    {
        public IReadOnlyCollection<DomainEvent> DomainEvents { get; }

        public void RaiseDomainEvent(DomainEvent domainEvent);
        public void ClearDomainEvents();
    }
}
