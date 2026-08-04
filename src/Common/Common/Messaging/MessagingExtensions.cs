using Common.Messaging.Events;
using Common.Messaging.Events.Sources;
using Common.Messaging.Options;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Messaging
{
    public static class MessagingExtensions
    {
        public static void AddMessaging(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            var configOptions = configurationManager.GetSection(nameof(Messaging)).Get<Messeging>() ??
                throw new ApplicationException( "Messaging configuration section is missing");

            services.AddEasyNetQ(configOptions.ConnectionString).UseSystemTextJson();
        }

        public static void AddMessagingHandlers(this IServiceCollection services)
        {
            services.Scan(s => s.FromEntryAssembly()
                .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddScoped<IDomainEventBuffer, DomainEventBuffer>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        }
    }
}
