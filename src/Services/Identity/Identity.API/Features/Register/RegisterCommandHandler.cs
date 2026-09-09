namespace Identity.API.Features.Register
{
    public record RegisterCommandResult(Guid UserId);
    public record RegisterCommand(string Username, string Email, string Password) : ICommand<RegisterCommandResult>;

    public class RegisterCommandHandler(IIdentityService IdentityService) : ICommandHandler<RegisterCommand, RegisterCommandResult>
    {
        public async Task<RegisterCommandResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
                UserName = request.Username,
                Email = request.Email,
            };

            await IdentityService.CreateUser(user, request.Password, cancellationToken);

            return new RegisterCommandResult(user.Id);
        }
    }
}
