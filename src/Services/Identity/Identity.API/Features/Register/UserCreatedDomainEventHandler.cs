namespace Identity.API.Features.Register
{
    public record UserCreatedDomainEvent(Guid Id, string Username, string Email) : DomainEvent(Id);

    public class UserCreatedDomainEventHandler : IEventHandler<UserCreatedDomainEvent>
    {
        public Task HandleAsync(UserCreatedDomainEvent notification)
        {
            return Task.CompletedTask;
        }
    }
}
