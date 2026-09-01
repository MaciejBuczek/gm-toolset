namespace Common.Messaging.Events.Sources
{
    public class IntegrationEventsCollector : IIntegrationEventsCollector
    {
        private readonly List<IntegrationEvent> _events = [];

        public void AddEvent(IntegrationEvent source)
        {
            _events.Add(source);
        }

        public void AddEvents(IEnumerable<IntegrationEvent> sources)
        {
            _events.AddRange(sources);
        }

        public void ClearEvents()
        {
            _events.Clear();
        }

        public IReadOnlyCollection<IntegrationEvent> GetEvents()
        {
            return _events.AsReadOnly();
        }
    }
}
