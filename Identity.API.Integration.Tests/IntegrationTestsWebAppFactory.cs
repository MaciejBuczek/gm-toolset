namespace Identity.API.Integration.Tests
{
    public class IntegrationTestsWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {

        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:latest")
            .WithDatabase("identity")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("ConnectionStrings:Database", _dbContainer.GetConnectionString());
            builder.UseSetting("Jwt:Issuer", "identity.api");
            builder.UseSetting("Jwt:Audience", "gm-toolset");
            builder.UseSetting("Jwt:SecretKey", "supersecretkeyyoushouldnotcommit");
            builder.UseSetting("Jwt:ExpirationInMinutes", "10");
            builder.UseSetting("Jwt:RefreshTokenExpirationInDays", "7");
        }

        public Task InitializeAsync()
        {
            return _dbContainer.StartAsync();
        }

        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }
    }
}
