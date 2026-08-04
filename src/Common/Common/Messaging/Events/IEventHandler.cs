namespace Common.Messaging.Events
{
    public interface IEventHandler<in TEvent>
    {
        Task HandleAsync(TEvent @event);
    }
}
