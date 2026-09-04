namespace Identity.API.Services
{
    public class IdentityService(UserManager<AppUser> UserManager, ITransactionHandler TransactionHandler, IDomainEventCollector DomainEventBuffor) : IIdentityService
    {
        public async Task CreateUser(AppUser user, string password, CancellationToken cancellationToken = default)
        {
            if(string.IsNullOrEmpty(user?.UserName) || string.IsNullOrEmpty(user?.Email))
            {
                throw new ArgumentNullException(nameof(user));
            }

            using var transaction = await TransactionHandler.BeginTransactionAsync(cancellationToken);

            var identityResult = await UserManager.CreateAsync(user, password);
            if (!identityResult.Succeeded)
            {
                throw new BadRequestException(string.Join(Constants.ExceptionSeparator, identityResult.Errors.Select(e => e.Description)));
            }

            var roleResult = await UserManager.AddToRoleAsync(user, Constants.Roles.User);
            if (!roleResult.Succeeded)
            {
                throw new BadRequestException(string.Join(Constants.ExceptionSeparator, roleResult.Errors.Select(e => e.Description)));
            }

            await transaction.CommitAsync(cancellationToken);

            user.RaiseDomainEvent(new UserCreatedDomainEvent(Guid.NewGuid(), user.UserName, user.Email));
            DomainEventBuffor.AddEventSource(user);
        }
    }
}
