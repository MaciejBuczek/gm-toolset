namespace Identity.API.Services.Contracts
{
    public interface IIdentityService
    {
        Task CreateUser(AppUser user, string password, CancellationToken cancellationToken = default);
    }
}
