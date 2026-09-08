namespace Identity.API.UnitTests.Services
{
    public class IdentityServiceTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock = SetupHelper.CreateUserManagerMock();
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock =SetupHelper.CreateRoleManagerMock();
        private readonly Mock<ITransactionHandler> _transactionHandlerMock = new();
        private readonly Mock<IDomainEventCollector> _domainEventCollectorMock = new();

        private readonly AppUser _appUser = new()
        {
            UserName = "TestUser",
            Email = "testing@email.com"
        };
        private readonly string _password = "TestPassword123!";

        private IdentityService CreateIdentityService()
        {
            return new IdentityService(
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _transactionHandlerMock.Object,
                _domainEventCollectorMock.Object);
        }

        [Fact]
        public async Task CreateUser_ShouldThrowArgumentNullException_WhenUserIsNull()
        {
            // Arrange
            var identityService = CreateIdentityService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await identityService.CreateUser(null, _password));
        }

        [Fact]
        public async Task CreateUser_ShouldThrowArgumentNullException_WhenEmailOrUserNameIsNull()
        {
            // Arrange
            var identityService = CreateIdentityService();
            var userWithNullEmail = new AppUser { UserName = "TestUser", Email = null };
            var userWithNullUserName = new AppUser { UserName = null, Email = "testing@email.com" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await identityService.CreateUser(userWithNullEmail, _password));
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await identityService.CreateUser(userWithNullUserName, _password));
        }

        [Fact]
        public async Task CreateUser_ShouldThrowIdentityExceptionWhenUserCreationFails()
        {
            // Arrange
            var exceptionMessage = "User creation failed";
            var identityService = CreateIdentityService();
            var identityResult = IdentityResult.Failed(new IdentityError { Description = exceptionMessage });
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(identityResult);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<IdentityException>(async () => await identityService.CreateUser(_appUser, _password));
            Assert.Contains(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task CreateUser_ShouldThrowIdentityExceptionWhenRoleAssignmentFails()
        {
            // Arrange
            var exceptionMessage = "Role assignment failed";
            var identityService = CreateIdentityService();
            var identityResult = IdentityResult.Success;
            var roleResult = IdentityResult.Failed(new IdentityError { Description = exceptionMessage });
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(identityResult);
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(roleResult);
            _roleManagerMock.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<IdentityException>(async () => await identityService.CreateUser(_appUser, _password));
            Assert.Contains(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task CreateUser_ShouldThrowIdentityRoleNotFoundExceptionWhenRoleIsNotFound()
        {
            // Arrange
            var identityService = CreateIdentityService();
            var identityResult = IdentityResult.Success;
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(identityResult);
            _roleManagerMock.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<IdentityRoleNotFoundException>(async () => await identityService.CreateUser(_appUser, _password));
        }

        [Fact]
        public async Task CreateUser_ShouldCommitTransactionAndRaiseDomainEvent_WhenUserCreationAndRoleAssignmentSucceed()
        {
            // Arrange
            var identityService = CreateIdentityService();
            var identityResult = IdentityResult.Success;
            var roleResult = IdentityResult.Success;
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(identityResult);
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(roleResult);
            _roleManagerMock.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _transactionHandlerMock.Setup(th => th.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            // Act
            await identityService.CreateUser(_appUser, _password);
            // Assert
            _transactionHandlerMock.Verify(th => th.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _domainEventCollectorMock.Verify(de => de.AddEventSource(It.Is<AppUser>(u => u == _appUser)), Times.Once);        }
    }
}
