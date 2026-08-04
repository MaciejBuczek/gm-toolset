namespace Identity.API.Events
{
    public record UserCreatedDomainEvent(Guid Id, string Username, string Email) : DomainEvent(Id);
}
