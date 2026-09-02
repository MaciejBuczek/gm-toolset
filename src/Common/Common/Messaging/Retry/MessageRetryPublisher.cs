using Common.Messaging.Retry.Contracts;
using EasyNetQ;
using EasyNetQ.Consumer;

namespace Common.Messaging.Retry
{
    public sealed class MessageRetryPublisher(IAdvancedBus AdvancedBus) : IMessageRetryPublisher
    {
        public async Task PublishAsync(ConsumeContext context, TimeSpan delay, CancellationToken cancellationToken = default)
        {
            var retryQueueName =
            $"{context.ReceivedInfo.Queue}.retry.{delay.TotalSeconds}s";

            await AdvancedBus.ExchangeDeclareAsync(
                RetryConstants.RetryExchangeName,
                ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: new Dictionary<string, object>(),
                cancellationToken);

            await AdvancedBus.QueueDeclareAsync(
                retryQueueName,
                configuration =>
                {
                    configuration.AsDurable(true);
                    configuration.AsExclusive(false);
                    configuration.AsAutoDelete(false);
                    configuration.WithArgument(RetryConstants.RetryTTLHeaderName, (long)delay.TotalMilliseconds);
                    configuration.WithArgument(RetryConstants.RetryDeadLetterExchangeHeaderName, context.ReceivedInfo.Exchange);
                    configuration.WithArgument(RetryConstants.RetryDeadLetterRoutingKeyHeaderName, context.ReceivedInfo.RoutingKey);
                }, cancellationToken);

            await AdvancedBus.QueueBindAsync( retryQueueName, RetryConstants.RetryExchangeName, retryQueueName,
                new Dictionary<string, object>(), cancellationToken);

            var headers = context.Properties.Headers is null
                ? [] : new Dictionary<string, object>(context.Properties.Headers);

            RetryHeaderHandler.SetRetryCount(headers, RetryHeaderHandler.GetRetryCount(headers) + 1);
            var properties = context.Properties with
            {
                Headers = headers
            };

            await AdvancedBus.PublishAsync(RetryConstants.RetryExchangeName, retryQueueName, mandatory: false,
                publisherConfirms: true, properties, context.Body, cancellationToken);
        }
    }
}
