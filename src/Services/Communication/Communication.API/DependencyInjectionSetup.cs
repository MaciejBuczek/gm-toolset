namespace Communication.API
{
    public static class DependencyInjectionSetup
    {
        internal static IServiceCollection SetUpDI(this IServiceCollection services, ConfigurationManager configurationManager, bool isDevelopment)
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
                .AddCarter()
                .AddLocalConsumers(assembly)
                .AddServices(configurationManager, isDevelopment)
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            return services;
        }

        private static IServiceCollection AddLocalConsumers(this IServiceCollection services, Assembly assembly)
        {
            return services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IConsumeAsync<>)), publicOnly: false)
                .AsSelf()
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }

        private static IServiceCollection AddServices(this IServiceCollection services, ConfigurationManager configurationManager, bool isDevelopment)
        {
            services.AddHostedService<AutoSubscriberHostedService>();
            services.AddScoped<IEmailTemplateRenderer, RazorRenderer>();

            if(isDevelopment)
            {
                services.AddScoped<IEmailSender, MockedEmailSender>();
            }
            else
            {
                services.AddScoped<IEmailSender, CommunicationServiceEmailSender>();
            }

            services.Configure<AzureCommunicationService>(configurationManager.GetSection(nameof(AzureCommunicationService)));
            services.AddSingleton(acs =>
            {
                var options = acs
                    .GetRequiredService<IOptions<AzureCommunicationService>>()
                    .Value;

                return new EmailClient(options.ConnectionString);
            });
            return services;
        }
    }
}
