using Common.Messaging.Events;
using Common.Messaging.Events.Services;
using Common.Messaging.Events.Sources;

namespace Common.Mediator.Pipelines
{
    public sealed class DomainEventsRequestHandler<TRequest, TResponse>(
        IRequestHandler<TRequest, TResponse> InnerHandler,
        IEnumerable<IHasDomainEvents> EventSources,
        IEventDispatcher<DomainEvent> DomainEventDispatcher,
        IDomainEventCollector DomainEventBuffer) : IRequestHandler<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            var response = await InnerHandler.Handle(request, cancellationToken);

            var events = EventSources.SelectMany(x => x.DomainEvents).Concat(DomainEventBuffer.GetEvents());
            if(events.Any())
            {
                await DomainEventDispatcher.DispatchEventsAsync(events, cancellationToken);
            }
            foreach (var eventSource in EventSources)
            {
                eventSource.ClearDomainEvents();
            }

            return response;
        }
    }
}
