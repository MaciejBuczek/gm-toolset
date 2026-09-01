using Common.Messaging.Events;
using Common.Messaging.Events.Services;
using Common.Messaging.Events.Sources;

namespace Common.Mediator.Pipelines
{
    public sealed class IntegrationEventsRequestHandler<TRequest, TResponse>(
        IRequestHandler<TRequest, TResponse> InnerHandler,
        IIntegrationEventsCollector IntegrationEventsCollector,
        IEventDispatcher<IntegrationEvent> IntegrationEventDispatcher) : IRequestHandler<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            var response = await InnerHandler.Handle(request, cancellationToken);

            var integrationEvents = IntegrationEventsCollector.GetEvents();
            if (integrationEvents != null && integrationEvents.Count != 0)
            {
                await IntegrationEventDispatcher.DispatchEventsAsync(integrationEvents, cancellationToken);

                IntegrationEventsCollector.ClearEvents();
            }

            return response;
        }
    }
}
