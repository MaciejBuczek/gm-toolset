namespace Character.API.Integration.Tests
{
    public abstract class BaseIntegrationTest<T> : IClassFixture<IntegrationTestsWebAppFactory> where T: notnull
    {
        private readonly IServiceScope _scope;
        internal readonly T handler;
        internal ICharacterRepository repository;

        protected BaseIntegrationTest(IntegrationTestsWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();
            handler = _scope.ServiceProvider.GetRequiredService<T>();
            repository = _scope.ServiceProvider.GetRequiredService<ICharacterRepository>();
        }
    }
}
