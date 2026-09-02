using EasyNetQ.Consumer;
using Common.Messaging.Exceptions;
using EasyNetQ;
using Microsoft.Extensions.Logging;

namespace Common.Messaging.Retry
{
    public sealed class ErrorStrategyProvider(ILogger<DefaultConsumeErrorStrategy> Logger,
        IMessageRetryPublisher MessageRetryPublisher,
        IConsumerConnection Connection,
        ISerializer Serializer,
        IConventions Cconventions,
        ITypeNameSerializer TypeNameSerializer,
        IErrorMessageSerializer ErrorMessageSerializer,
        ConnectionConfiguration Configuration) :
        DefaultConsumeErrorStrategy(Logger, Connection, Serializer, Cconventions, TypeNameSerializer, ErrorMessageSerializer, Configuration)
    {
        public override async ValueTask<AckStrategyAsync> HandleErrorAsync(ConsumeContext context,  Exception exception, CancellationToken cancellationToken = default)
        {
            if (exception is RetryableException)
            {
                var retryCount = RetryHeaderHandler.GetRetryCount(context.Properties.Headers?? new Dictionary<string, object>());
                if (MessageRetryDelyProvider.TryGetRetryDelay(retryCount, out var delay))
                {
                    await MessageRetryPublisher.PublishAsync(context, delay,cancellationToken);
                    return AckStrategies.AckAsync;
                }
            }

            return await base.HandleErrorAsync(context, exception, cancellationToken);
        }
    }
}
