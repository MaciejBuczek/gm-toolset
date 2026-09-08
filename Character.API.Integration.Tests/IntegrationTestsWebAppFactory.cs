using Marten;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
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
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services
                    .FirstOrDefault(x => x.ServiceType == typeof(IDocumentStore));

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                services.AddMarten(options =>
                {
                    options.Connection(_dbContainer.GetConnectionString());
                    options.DatabaseSchemaName = "character";
                })
                .UseLightweightSessions();
            });
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
