namespace Identity.API.UnitTests
{
    internal static class SetupHelper
    {
        internal static Mock<UserManager<AppUser>> CreateUserManagerMock()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();

            return new Mock<UserManager<AppUser>>(
                userStoreMock.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);
        }

        internal static Mock<RoleManager<IdentityRole>> CreateRoleManagerMock()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();

            return new Mock<RoleManager<IdentityRole>>(
                store.Object,
                null,
                null,
                null,
                null);
        }
    }

}
