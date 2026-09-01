using Common.Messaging.Events.Sources;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Messaging.Events.Services
{
    public class DomainEventDispatcher(IServiceProvider ServiceProvider, IIntegrationEventsCollector IntegrationEventsCollector) : IEventDispatcher<DomainEvent>
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
                            var integrationEvents = (IEnumerable<IntegrationEvent>)((dynamic)projection).Project((dynamic)domainEvent);
                            if (integrationEvents != null)
                            {
                                IntegrationEventsCollector.AddEvents(integrationEvents);
                            }
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
