namespace Common.Messaging.Events
{
    public interface IIntegrationEventProjection<in TDomainEvent> where TDomainEvent: DomainEvent
    {
        IEnumerable<IntegrationEvent> Project(TDomainEvent domainEvent); 
    }
}
