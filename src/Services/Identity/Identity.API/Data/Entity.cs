using Common.Messaging.Events;

namespace Identity.API.Data
{
    public abstract class Entity
    {
        private readonly List<DomainEvent> _domainEvents = [];
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        private void RaiseDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}
