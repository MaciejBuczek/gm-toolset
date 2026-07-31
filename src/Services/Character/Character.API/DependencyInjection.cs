namespace Character.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMartenConnection(this IServiceCollection services, ConfigurationManager configurationManager)
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

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<Program>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddValidatorsFromAssembly(
                typeof(Program).Assembly,
                includeInternalTypes: true);

            services.Decorate(
                typeof(IRequestHandler<,>),
                typeof(ValidationRequestHandler<,>));
            services.Decorate(
                typeof(IRequestHandler<,>),
                typeof(LoggingRequestHandler<,>));

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            return app;
        }

        public static IServiceCollection AddCharacterRepository(this IServiceCollection services)
        {
            services.AddTransient<ICharacterRepository, CharacterRepository>();
            return services;
        }
    }
}