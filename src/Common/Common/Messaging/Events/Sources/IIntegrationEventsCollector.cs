namespace Common.Messaging.Events.Sources
{
    public interface IIntegrationEventsCollector
    {
        void AddEvent(IntegrationEvent source);
        void AddEvents(IEnumerable<IntegrationEvent> sources);
        IReadOnlyCollection<IntegrationEvent> GetEvents();
        void ClearEvents();
    }
}
