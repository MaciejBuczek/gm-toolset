using Microsoft.Extensions.DependencyInjection;

namespace Character.API.Integration.Tests
{
    public class BaseIntegrationTest(IntegrationTestsWebAppFactory Factory) : IClassFixture<IntegrationTestsWebAppFactory>
    {
        public readonly IServiceScope Scope = Factory.Services.CreateScope();
    }
}
