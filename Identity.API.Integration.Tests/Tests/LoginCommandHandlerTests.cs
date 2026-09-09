namespace Identity.API.Integration.Tests.Tests
{
    public class LoginCommandHandlerTests(IntegrationTestsWebAppFactory Factory) :
        BaseIntegrationTest<ICommandHandler<LoginCommand, LoginCommandResult>>(Factory)
    {
        private static readonly string _password = "Password123!";
        private static readonly string _userName = "TestUser";
        private static readonly string _email = "testemail@gmail.com";
        private static AppUser GetUser()
        {
            return new AppUser
            {
                UserName = _userName,
                Email = _email
            };
        }

        private readonly LoginCommand _loginCommand = new(Username: _userName, Email: _email, Password: _password);

        [Fact]
        public async Task Handle_ShouldReturnUnauthorizedExceptionWhenUserIsNotFound()
        {
            //Arrange, Act, Assert
            await Assert.ThrowsAsync<UnauthorizedException>(async () => await handler.Handle(_loginCommand));
        }

        [Fact]
        public async Task Handle_ShouldReturnUnauthorizedExceptionWhenPasswordIsIncorrect()
        {
            //Arrange
            var user = GetUser();
            var command = new LoginCommand(Username: _loginCommand.Username, Email: _loginCommand.Email, Password: "ThisIsAnIncorrectPassword1!");
            await userManager.CreateAsync(user, _password);

            //Act
            await Assert.ThrowsAsync<UnauthorizedException>(async () => await handler.Handle(command));
        }

        [Fact]
        public async Task Handle_ShouldReturnLoginCommandResult()
        {
            //Arrange
            var user = GetUser();
            await userManager.CreateAsync(user, _password);

            //Act
            var result = await handler.Handle(_loginCommand);

            //Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.False(string.IsNullOrEmpty(result.RefreshToken));
        }
    }
}
