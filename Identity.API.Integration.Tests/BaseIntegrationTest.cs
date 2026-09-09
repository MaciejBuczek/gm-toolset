namespace Identity.API.Integration.Tests
{
    public abstract class BaseIntegrationTest<T> : IClassFixture<IntegrationTestsWebAppFactory> where T : notnull
    {
        private readonly IServiceScope _scope;
        internal readonly T handler;
        internal readonly AppDbContext dbContext;
        internal UserManager<AppUser> userManager;

        protected BaseIntegrationTest(IntegrationTestsWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();
            handler = _scope.ServiceProvider.GetRequiredService<T>();
            dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
            userManager = _scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        }
    }
}
