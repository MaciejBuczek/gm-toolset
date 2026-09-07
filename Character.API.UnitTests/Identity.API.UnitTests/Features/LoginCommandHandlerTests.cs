namespace Identity.API.UnitTests.Features
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock = SetupHelper.CreateUserManagerMock();
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock = new ();
        private readonly Mock<ITokenGeneratorService> _tokenGeneratorServiceMock = new();
        private readonly LoginCommand _loginCommand = new(
            Username: "testuser",
            Email: "testuser@example.com",
            Password: "Password123!"
        );

        private LoginCommandHandler CreateHanlder()
        {
            return new LoginCommandHandler(
                _userManagerMock.Object,
                _refreshTokenRepositoryMock.Object,
                _tokenGeneratorServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnLoginCommandResult_WhenUserIsAuthenticated()
        {
            // Arrange
            var command = _loginCommand;
            var user = new AppUser { UserName = command.Username, Email = command.Email };
            var token = "test_token";
            var refreshToken = "test_refresh_token";
            _userManagerMock.Setup(x => x.FindByNameAsync(command.Username)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, command.Password)).ReturnsAsync(true);
            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(["User"]);
            _tokenGeneratorServiceMock.Setup(x => x.GenerateToken(user.Id, user.UserName, user.Email, It.IsAny<IEnumerable<string>>())).Returns(token);
            _tokenGeneratorServiceMock.Setup(x => x.GenerateRefreshToken()).Returns(refreshToken);
            var handler = CreateHanlder();

            // Act
            var result = await handler.Handle(command, default);
            // Assert

            Assert.NotNull(result);
            Assert.Equal(token, result.Token);
            Assert.Equal(refreshToken, result.RefreshToken);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedException_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var command = _loginCommand;
            _userManagerMock.Setup(x => x.FindByNameAsync(command.Username)).ReturnsAsync(null as AppUser);
            var handler = CreateHanlder();

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedException_WhenPasswordIsIncorrect()
        {
            // Arrange
            var command = _loginCommand;
            var user = new AppUser { UserName = command.Username, Email = command.Email };
            _userManagerMock.Setup(x => x.FindByNameAsync(command.Username)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, command.Password)).ReturnsAsync(false);
            var handler = CreateHanlder();

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedException_WhenUserIsNotFound()
        {
            // Arrange
            var command = _loginCommand;
            _userManagerMock.Setup(x => x.FindByNameAsync(command.Username)).ReturnsAsync(null as AppUser);
            _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync(null as AppUser);
            var handler = CreateHanlder();

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(async () => await handler.Handle(command, default));
        }
    }
}
