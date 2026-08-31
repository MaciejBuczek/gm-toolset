namespace Common.Messaging.Events.Services
{
    public interface IEventHandler<in TEvent>
    {
        Task HandleAsync(TEvent notification);
    }
}
