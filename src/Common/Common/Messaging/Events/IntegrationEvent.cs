namespace Common.Messaging.Events
{
    public abstract record IntegrationEvent(Guid Id) : DomainEvent(Id);
}
