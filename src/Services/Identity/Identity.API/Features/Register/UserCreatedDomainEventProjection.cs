namespace Identity.API.Features.Register
{
    public class UserCreatedDomainEventProjection : IIntegrationEventProjection<UserCreatedDomainEvent>
    {
        public IEnumerable<IntegrationEvent> Project(UserCreatedDomainEvent domainEvent)
        {
            yield return new UserCreatedIntegrationEvent(
                Id: domainEvent.Id,
                Username: domainEvent.Username,
                Email: domainEvent.Email);
        }
    }
}
