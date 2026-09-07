namespace Identity.API.UnitTests.Features
{
    public class LoginUsingRefreshTokenCommandHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock = SetupHelper.CreateUserManagerMock();
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock = new();
        private readonly Mock<ITokenGeneratorService> _tokenGeneratorServiceMock = new();
        private readonly LoginUsingRefreshTokenCommand _command = new(
            RefreshToken: "test_refresh_token"
        );

        private LoginUsingRefreshTokenCommandHandler CreateHanlder()
        {
            return new LoginUsingRefreshTokenCommandHandler(
                _userManagerMock.Object,
                _refreshTokenRepositoryMock.Object,
                _tokenGeneratorServiceMock.Object);
        }
        [Fact]
        public async Task Handle_ShouldReturnLoginUsingRefreshTokenCommandResult_WhenRefreshTokenIsValid()
        {
            // Arrange
            var command = _command;
            var newToken = "new_test_token";
            var newRefreshToken = "new_test_token";
            var user = new AppUser { Id = Guid.NewGuid() };

            _refreshTokenRepositoryMock.Setup(r => r.FindRefreshTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RefreshToken { Token = command.RefreshToken, ExpirationDate = DateTime.UtcNow.AddDays(1), User = user });
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(["User"]);
            _tokenGeneratorServiceMock.Setup(t => t.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(newToken);
            _tokenGeneratorServiceMock.Setup(t => t.GenerateRefreshToken())
                .Returns(newRefreshToken);
            var handler = CreateHanlder();

            //Act & Assert
            var result = await handler.Handle(command, default);

            Assert.NotNull(result);
            Assert.Equal(result.Token, newToken);
            Assert.Equal(result.RefreshToken, newRefreshToken);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedExceptionWhenRefreshTokenIsNotFound()
        {
            // Arrange
            var command = _command;
            _refreshTokenRepositoryMock.Setup(r => r.FindRefreshTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as RefreshToken);
            var handler = CreateHanlder();

            //Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(() => handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedExceptionWhenRefreshTokenIsExpired()
        {
            // Arrange
            var command = _command;
            var user = new AppUser { Id = Guid.NewGuid() };
            _refreshTokenRepositoryMock.Setup(r => r.FindRefreshTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RefreshToken { Token = command.RefreshToken, ExpirationDate = DateTime.UtcNow.AddDays(-1), User = user });
            var handler = CreateHanlder();

            //Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(() => handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedExceptionWhenUserIsMissing()
        {
            // Arrange
            var command = _command;
            _refreshTokenRepositoryMock.Setup(r => r.FindRefreshTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RefreshToken { Token = command.RefreshToken, ExpirationDate = DateTime.UtcNow.AddDays(1), User = null });
            var handler = CreateHanlder();

            //Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(() => handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedExceptionWhenUserIsNotFound()
        {
            // Arrange
            var command = _command;
            var user = new AppUser { Id = Guid.NewGuid() };
            _refreshTokenRepositoryMock.Setup(r => r.FindRefreshTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RefreshToken { Token = command.RefreshToken, ExpirationDate = DateTime.UtcNow.AddDays(1), User = null });
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(null as AppUser);
            var handler = CreateHanlder();

            //Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(() => handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldThrowIdentityRoleNotFoundExceptionWhenUserHasNoRoles()
        {
            // Arrange
            var command = _command;
            var user = new AppUser { Id = Guid.NewGuid() };

            _refreshTokenRepositoryMock.Setup(r => r.FindRefreshTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RefreshToken { Token = command.RefreshToken, ExpirationDate = DateTime.UtcNow.AddDays(1), User = user });
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<AppUser>()))
                .ReturnsAsync([]);
            var handler = CreateHanlder();

            //Act & Assert
            await Assert.ThrowsAsync<IdentityRoleNotFoundException>(() => handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldThrowIdentityExceptionWhenDbOperationsFail()
        {
            // Arrange
            var command = _command;
            var newToken = "new_test_token";
            var newRefreshToken = "new_test_token";
            var user = new AppUser { Id = Guid.NewGuid() };

            _refreshTokenRepositoryMock.Setup(r => r.FindRefreshTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RefreshToken { Token = command.RefreshToken, ExpirationDate = DateTime.UtcNow.AddDays(1), User = user });
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(["User"]);
            _tokenGeneratorServiceMock.Setup(t => t.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(newToken);
            _tokenGeneratorServiceMock.Setup(t => t.GenerateRefreshToken())
                .Returns(newRefreshToken);
            _refreshTokenRepositoryMock.Setup(t => t.OverwriteRefreshTokenAsync(It.IsAny<RefreshToken>(), It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws<DbUpdateException>();
            var handler = CreateHanlder();

            //Act & Assert
            await Assert.ThrowsAsync<IdentityException>(() => handler.Handle(command, default));
        }
    }
}
