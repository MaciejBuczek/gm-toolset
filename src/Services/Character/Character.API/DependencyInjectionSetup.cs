namespace Character.API
{
    internal static class DependencyInjectionSetup
    {
        internal static IServiceCollection SetUpDI(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            var assembly = typeof(DependencyInjectionSetup).Assembly;

            services.AddMartenConnection(configurationManager)
                .AddCharacterRepository()
                .AddRequestHandlers(assembly)
                .AddValidators(assembly)
                .DecorateRequestWithValidation()
                .DecorateRequestWithLogging()
                .AddExceptionHandler<CustomExceptionHandler>()
                .AddIdentity(configurationManager)
                .AddCarter();

            return services;
        }

        private static IServiceCollection AddMartenConnection(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            var appSetingsConfig = configurationManager.GetSection(nameof(Database)).Get<Database>();

            if (string.IsNullOrEmpty(appSetingsConfig?.ConnectionString))
            {
                throw new ApplicationException("Database configuration is missing");
            }
            if (string.IsNullOrEmpty(appSetingsConfig?.Schema))
            {
                throw new ApplicationException("Schema name is empty");
            }

            services.AddMarten(config => 
            {
                config.Connection(appSetingsConfig.ConnectionString);
                config.DatabaseSchemaName = appSetingsConfig.Schema;

            }).UseLightweightSessions();

            return services;
        }

        private static IServiceCollection AddCharacterRepository(this IServiceCollection services)
        {
            services.AddTransient<ICharacterRepository, CharacterRepository>();
            return services;
        }
    }
}