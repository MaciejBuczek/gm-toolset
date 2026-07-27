using Common.Mediator.Pipelines;

namespace Identity.API.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static void SetupDI(this IServiceCollection services)
        {
            services.AddSingleton<DbSetupHelper>();
        }

        public static void SetupIdentity(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
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
    }
}
