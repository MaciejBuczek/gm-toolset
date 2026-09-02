using EasyNetQ.Consumer;

namespace Common.Messaging.Retry
{
    public interface IMessageRetryPublisher
    {
        Task PublishAsync(ConsumeContext context, TimeSpan delay, CancellationToken cancellationToken = default);
    }
}
