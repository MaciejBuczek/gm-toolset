namespace Identity.API
{
    internal static class DependencyInjectionSetup
    {
        internal static IServiceCollection SetUpDI(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            var assembly = typeof(DependencyInjectionSetup).Assembly;
            services
                 .AddValidators(assembly)
                 .AddExceptionHandler<CustomExceptionHandler>()
                 .AddMessaging(configurationManager)
                 .AddMessagingHandlers()
                 .AddRequestHandlers(assembly)
                 .DecorateRequestWithEventHandling()
                 .DecorateRequestWithValidation()
                 .DecorateRequestWithLogging()
                 .AddCarter()
                 .SetupOptions(configurationManager)
                 .SetupIdentityContext(configurationManager)
                 .AddServices();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<DbSetupHelper>();
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }

        public static IServiceCollection SetupIdentityContext(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            var dbOptions = configurationManager.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>();
            var connectionString = dbOptions?.Database?? string.Empty;

            if(connectionString != string.Empty)
            {
                services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString, options =>
                {
                    options.MigrationsHistoryTable(Constants.DefaultMigrationsHistoryTable, Constants.DefaultSchema);
                }));
                services.AddIdentity<AppUser, IdentityRole<Guid>>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();
            }          

            return services;
        }

        private static IServiceCollection SetupOptions(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            return services.Configure<Jwt>(configurationManager.GetSection(nameof(Jwt)))
                .Configure<ConnectionStrings>(configurationManager.GetSection(nameof(ConnectionStrings)));
        }
    }
}
