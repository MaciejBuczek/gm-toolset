namespace Identity.API.Events.Handlers
{
    public class UserCreatedDomainEventHandler : IEventHandler<UserCreatedDomainEvent>
    {
        public Task HandleAsync(UserCreatedDomainEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
