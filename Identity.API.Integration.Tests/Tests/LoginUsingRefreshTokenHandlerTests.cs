namespace Identity.API.Integration.Tests.Tests
{
    public class LoginUsingRefreshTokenHandlerTests(IntegrationTestsWebAppFactory Factory) :
        BaseIntegrationTest<ICommandHandler<LoginUsingRefreshTokenCommand, LoginUsingRefreshTokenCommandResult>>(Factory)
    {
        private readonly LoginUsingRefreshTokenCommand _command = new(RefreshToken: "1xzo9qNA7rC3xRT0GI+6WdQsulUjOBTNbifxx92Vvwg=");
        private static readonly AppUser _user = new()
        {
            UserName = "TestUser",
            Email = "testemail@gmail.com",
        };

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedExceptionIfTokenIsNotFoundInDatabase()
        {
            //Arrange Act Assert
            await Assert.ThrowsAsync<UnauthorizedException>(async () => await handler.Handle(_command));
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedExceptionIfRefreshTokenUserIsNotFound()
        {
            //Arrange
            await userManager.CreateAsync(_user);
            await refreshTokenRepository.SaveRefreshTokenToDbAsync(_user, _command.RefreshToken);
            await userManager.DeleteAsync(_user);

            //Act
            await Assert.ThrowsAsync<UnauthorizedException>(async () => await handler.Handle(_command));
        }

        [Fact]
        public async Task Handle_ShouldThrowIdentityRoleNotFoundExceptionIfRefreshTokenUserHasNoRoles()
        {
            //Arrange
            await userManager.CreateAsync(_user);
            await refreshTokenRepository.SaveRefreshTokenToDbAsync(_user, _command.RefreshToken);

            //Act
            await Assert.ThrowsAsync<IdentityRoleNotFoundException>(async () => await handler.Handle(_command));
        }

        [Fact]
        public async Task Handle_ShouldReturnLoginUsingRefreshTokenResult()
        {
            //Arrange
            await userManager.CreateAsync(_user);
            await refreshTokenRepository.SaveRefreshTokenToDbAsync(_user, _command.RefreshToken);
            await userManager.AddToRoleAsync(_user, "user");

            //Act
            await handler.Handle(_command);

            //Assert
            Assert.NotNull(_command);
            Assert.False(string.IsNullOrEmpty(_command.RefreshToken));
        }
    }
}
