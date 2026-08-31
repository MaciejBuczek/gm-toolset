using Common.Messaging.Events;
using Common.Messaging.Events.Services;

namespace Common.Mediator.Pipelines
{
    public sealed class IntegrationEventsRequestHandler<TRequest, TResponse>(
        IRequestHandler<TRequest, TResponse> InnerHandler,
        IEnumerable<IntegrationEvent> events,
        IEventDispatcher<IntegrationEvent> IntegrationEventDispatcher) : IRequestHandler<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            var response = await InnerHandler.Handle(request, cancellationToken);

            if (events.Any())
            {
                await IntegrationEventDispatcher.DispatchEventsAsync(events, cancellationToken);
            }

            return response;
        }
    }
}
