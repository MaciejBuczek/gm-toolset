namespace Identity.API.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static void SetupDI(this IServiceCollection services)
        {
            services.AddSingleton<DbSetupHelper>();
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        }

        public static void SetupOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Jwt>(configuration.GetSection(nameof(Jwt)));
        }

        public static void SetupIdentity(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddIdentity<AppUser, IdentityRole<Guid>>()
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
