using Identity.API.Features.Register;

namespace Identity.API.UnitTests.Features
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IIdentityService> _identityService = new();
        private readonly RegisterCommand _registerCommand = new(
            Username: "testuser",
            Email: "testuser@example.com",
            Password: "Password123!"
        );

        [Fact]
        public async Task Handle_ShouldReturnRegisterCommandResult_WhenUserIsCreated()
        {
            // Arrange
            var command = _registerCommand;
            _identityService.Setup(x => x.CreateUser(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new RegisterCommandHandler(_identityService.Object);
            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeded);
        }
    }
}
