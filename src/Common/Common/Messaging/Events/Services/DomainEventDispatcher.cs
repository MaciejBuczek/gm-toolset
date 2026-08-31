using Microsoft.Extensions.DependencyInjection;

namespace Common.Messaging.Events.Services
{
    public class DomainEventDispatcher(IServiceProvider ServiceProvider) : IEventDispatcher<DomainEvent>
    {
        public async Task DispatchEventsAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken = default)
        {
            foreach(var domainEvent in events)
            {
                var handlers = GetHandlersForEvent(domainEvent);
                if (handlers != null && handlers.Any())
                {
                    foreach (var handler in handlers)
                    {
                        if (handler != null)
                        {
                            await ((dynamic)handler).HandleAsync((dynamic)domainEvent);
                        }
                    }
                }

                var projections = GetIntegrationProjections(domainEvent);
                if (projections != null && projections.Any())
                {
                    foreach (var projection in projections)
                    {
                        if (projection != null)
                        {
                            ((dynamic)projection).Project((dynamic)domainEvent);
                        }
                    }
                }
            }
        }

        private IEnumerable<object?> GetHandlersForEvent(DomainEvent domainEvent)
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());
            return ServiceProvider.GetServices(handlerType);
        }

        private IEnumerable<object?> GetIntegrationProjections(DomainEvent domainEvent)
        {
            var projectionType = typeof(IIntegrationEventProjection<>).MakeGenericType(domainEvent.GetType());
            return ServiceProvider.GetServices(projectionType);
        }
    }
}
