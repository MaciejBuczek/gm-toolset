using Common.Exceptions.Handler;
using Common.Identity;
using Common.Mediator;

namespace Communication.API
{
    public static class DependencyInjection
    {
        public static void SetUpDI(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddExceptionHandler<CustomExceptionHandler>()
                .AddRequestHandlers()
                .AddValidators()
                .DecorateRequestWithEventHandling()
                .DecorateRequestWithValidation()
                .DecorateRequestWithLogging()
                .AddIdentity(configurationManager);
        }
    }
}
