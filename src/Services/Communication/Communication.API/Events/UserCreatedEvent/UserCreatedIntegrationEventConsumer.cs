using Common.Messaging.Events.Contracts.Identity;
using EasyNetQ.AutoSubscribe;

namespace Communication.API.Events.UserCreatedEvent
{
    public class UserCreatedIntegrationEventConsumer : IConsumeAsync<UserCreatedIntegrationEvent>
    {
        public Task ConsumeAsync(UserCreatedIntegrationEvent message, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"UserCreatedIntegrationEvent received: UserId={message.Id}, Email={message.Email}");
            return Task.CompletedTask;
        }
    }
}
