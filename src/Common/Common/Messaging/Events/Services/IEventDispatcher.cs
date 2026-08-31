namespace Common.Messaging.Events.Services
{
    public interface IEventDispatcher<T> where T: IEvent
    {
        Task DispatchEventsAsync(IEnumerable<T> events, CancellationToken cancellationToken = default);
    }
}
