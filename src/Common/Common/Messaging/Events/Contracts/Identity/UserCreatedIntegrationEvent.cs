namespace Common.Messaging.Events.Contracts.Identity
{
    public record UserCreatedIntegrationEvent(Guid Id, string Username, string Email) : IntegrationEvent(Id)
    {
    }
}
