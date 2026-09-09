namespace Identity.API.Integration.Tests.Tests
{
    public class RegisterCommandHandlerTests(IntegrationTestsWebAppFactory Factory) :
        BaseIntegrationTest<ICommandHandler<RegisterCommand, RegisterCommandResult>>(Factory)
    {
        private readonly RegisterCommand _command = new(Username: "TestUser", Email: "testemail@gmail.com", Password: "Password123!");

        [Fact]
        public async Task Handle_ShouldThrowIdentityExceptionWhenPasswordDoesNotMachRules()
        {
            var command = new RegisterCommand(Username: _command.Username, Email: _command.Email, Password: "123");

            //Arrange Act Assert
            await Assert.ThrowsAsync<IdentityException>(async () => await handler.Handle(_command));
        }

        [Fact]
        public async Task Handle_ShouldCreateUserInDatabase()
        {
            //Arrange, Act
            var result = await handler.Handle(_command);
            var dbResult = await dbContext.FindAsync<AppUser>(result.UserId);

            //Assert
            Assert.NotNull(dbResult);
            Assert.Equal(dbResult.UserName, _command.Username);
            Assert.Equal(dbResult.Email, _command.Email);
        }
    }
}
