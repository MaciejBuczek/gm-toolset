using Carter;
using Common.Exceptions.Handler;
using Common.Mediator;
using Common.Messaging;
using Communication.API.Services;
using EasyNetQ.AutoSubscribe;

namespace Communication.API
{
    public static class DependencyInjectionSetup
    {
        internal static IServiceCollection SetUpDI(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            var assembly = typeof(DependencyInjectionSetup).Assembly;

            services
                .AddValidators(assembly)
                .AddExceptionHandler<CustomExceptionHandler>()
                .AddIdentity(configurationManager)
                .AddMessaging(configurationManager)
                .AddMessagingHandlers()
                .AddRequestHandlers(assembly)
                .DecorateRequestWithEventHandling()
                .DecorateRequestWithValidation()
                .DecorateRequestWithLogging()
                .AddCarter();

            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IConsumeAsync<>)), publicOnly: false)
                .AsSelf()
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddHostedService<AutoSubscriberHostedService>();

            return services;
        }
    }
}
