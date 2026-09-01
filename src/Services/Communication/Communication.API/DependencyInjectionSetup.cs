using Carter;
using Common.Exceptions.Handler;
using Common.Mediator;
using Common.Messaging;

namespace Communication.API
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection SetUpDI(this IServiceCollection services, ConfigurationManager configurationManager)
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

            return services;
        }
    }
}
