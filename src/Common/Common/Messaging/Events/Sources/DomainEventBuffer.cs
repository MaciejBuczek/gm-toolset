namespace Common.Messaging.Events.Sources
{
    internal class DomainEventBuffer : IDomainEventBuffer
    {
        private readonly List<IHasDomainEvents> _eventSources = [];

        public void AddEventSource(IHasDomainEvents source)
        {
            if(source != null)
            {
                _eventSources.Add(source);
            }
        }

        public void ClearEvents()
        {
            foreach (var source in _eventSources)
            {
                source.ClearDomainEvents();
            }
        }

        public IReadOnlyCollection<DomainEvent> GetEvents()
        {
            return _eventSources.SelectMany(e => e.DomainEvents).ToList().AsReadOnly();
        }
    }
}
