namespace Identity.API.Features.Register
{
    internal record RegisterCommandResult(bool Succeded);
    internal record RegisterCommand(string Username, string Email, string Password) : ICommand<RegisterCommandResult>;

    internal class RegisterCommandHandler(UserManager<AppUser> UserManager, AppDbContext DbContext) : ICommandHandler<RegisterCommand, RegisterCommandResult>
    {
        public async Task<RegisterCommandResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
                UserName = request.Username,
                Email = request.Email,
            };

            using var transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);

            var identityResult = await UserManager.CreateAsync(user, request.Password);
            if (!identityResult.Succeeded)
            {
                throw new BadRequestException(string.Join(Constants.ExceptionSeparator, identityResult.Errors.Select(e => e.Description)));
            }

            await UserManager.AddToRoleAsync(user, Constants.Roles.User);

            await transaction.CommitAsync(cancellationToken);

            return new RegisterCommandResult(identityResult.Succeeded);
        }
    }
}
