namespace Common.Messaging.Events
{
    public abstract record DomainEvent(Guid Id) : IEvent;
}
