using Marten;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace Character.API.Integration.Tests
{
    public class IntegrationTestsWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {

        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:latest")
            .WithDatabase("character")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("Database:ConnectionString",_dbContainer.GetConnectionString());
            builder.UseSetting("Database:Schema","character");
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
