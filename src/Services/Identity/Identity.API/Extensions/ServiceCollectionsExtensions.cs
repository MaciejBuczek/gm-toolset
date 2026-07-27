namespace Identity.API.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static void SetupDI(this IServiceCollection services)
        {
            services.AddSingleton<DbSetupHelper>();
        }
    }
}
