using Common.Exceptions.Handler;
using Common.Identity;
using Common.Mediator;
using Common.Messaging;

namespace Communication.API
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection SetUpDI(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services
                .AddValidators()
                .AddExceptionHandler<CustomExceptionHandler>()
                .AddIdentity(configurationManager)
                .AddMessaging(configurationManager)
                .AddMessagingHandlers()
                .AddRequestHandlers()
                .DecorateRequestWithEventHandling()
                .DecorateRequestWithValidation()
                .DecorateRequestWithLogging();

            return services;
        }
    }
}
