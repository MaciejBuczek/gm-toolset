namespace Identity.API.Helpers
{
    public class DbSetupHelper
    {
        private IServiceScope? ServiceScope { get; set; }

        private static IServiceScope ResolveServiceScope(WebApplication app)
        {
            return app is null ? throw new ArgumentNullException(nameof(app)) : app.Services.CreateScope();
        }

        public async Task EnsureMigrations(WebApplication app)
        {
            ServiceScope ??= ResolveServiceScope(app);

            var dbContext = ServiceScope.ServiceProvider.GetRequiredService<AppDbContext>()
                ?? throw new InvalidOperationException("AppDbContext is not registered in the service provider.");

            await dbContext.Database.MigrateAsync();
        }

        public async Task EnsureRolesAreCreated(WebApplication app)
        {

            ServiceScope ??= ResolveServiceScope(app);

            var roleManager = ServiceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>()
                ?? throw new InvalidOperationException("RoleManager<IdentityRole> is not registered in the service provider.");

            if (!await roleManager.RoleExistsAsync(Constants.Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(Constants.Roles.Admin));
            }
            if(!await roleManager.RoleExistsAsync(Constants.Roles.User))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(Constants.Roles.User));
            }
        }
    }
}
