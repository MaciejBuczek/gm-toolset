using Microsoft.Extensions.DependencyInjection;

namespace Common.Messaging.Events
{
    public class DomainEventDispatcher(IServiceProvider ServiceProvider) : IDomainEventDispatcher
    {
        public async Task DispatchEventsAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken = default)
        {
            foreach(var domainEvent in events)
            {
                var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());
                var handlers = ServiceProvider.GetServices(handlerType);
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
                else
                {
                    throw new InvalidOperationException($"No handler found for event type {domainEvent.GetType().Name}");
                }
            }
        }
    }
}
