using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Identity
{
    public static class CommonSetup
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddAuthorization();
            services.AddJwtAuthentication(configurationManager);

            return services;
        }
    }
}
