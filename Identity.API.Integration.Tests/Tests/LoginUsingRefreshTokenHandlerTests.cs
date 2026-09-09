namespace Identity.API.Integration.Tests.Tests
{
    public class LoginUsingRefreshTokenHandlerTests(IntegrationTestsWebAppFactory Factory) :
        BaseIntegrationTest<ICommandHandler<LoginUsingRefreshTokenCommand, LoginUsingRefreshTokenCommandResult>>(Factory)
    {
        private static string GetRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        private static AppUser GetUser()
        {
            var id = Guid.NewGuid();
            return new AppUser
            {
                UserName = $"TestUser{id}",
                Email = $"testemail@gmail.com{id}",
            };
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedExceptionIfTokenIsNotFoundInDatabase()
        {
            //Arrange
            var command = new LoginUsingRefreshTokenCommand(GetRefreshToken());

            //Act, Assert
            await Assert.ThrowsAsync<UnauthorizedException>(async () => await handler.Handle(command));
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedExceptionIfRefreshTokenUserIsNotFound()
        {
            //Arrange
            var command = new LoginUsingRefreshTokenCommand(GetRefreshToken());
            var user = GetUser();
            await userManager.CreateAsync(user);
            await refreshTokenRepository.SaveRefreshTokenToDbAsync(user, command.RefreshToken);
            await userManager.DeleteAsync(user);

            //Act
            await Assert.ThrowsAsync<UnauthorizedException>(async () => await handler.Handle(command));
        }

        [Fact]
        public async Task Handle_ShouldThrowIdentityRoleNotFoundExceptionIfRefreshTokenUserHasNoRoles()
        {

            //Arrange
            var command = new LoginUsingRefreshTokenCommand(GetRefreshToken());
            var user = GetUser();
            await userManager.CreateAsync(user);
            await refreshTokenRepository.SaveRefreshTokenToDbAsync(user, command.RefreshToken);

            //Act
            await Assert.ThrowsAsync<IdentityRoleNotFoundException>(async () => await handler.Handle(command));
        }

        [Fact]
        public async Task Handle_ShouldReturnLoginUsingRefreshTokenResult()
        {
            //Arrange
            var command = new LoginUsingRefreshTokenCommand(GetRefreshToken());
            var user = GetUser();
            await userManager.CreateAsync(user);
            await refreshTokenRepository.SaveRefreshTokenToDbAsync(user, command.RefreshToken);
            await userManager.AddToRoleAsync(user, "user");

            //Act
            await handler.Handle(command);

            //Assert
            Assert.NotNull(command);
            Assert.False(string.IsNullOrEmpty(command.RefreshToken));
        }
    }
}
