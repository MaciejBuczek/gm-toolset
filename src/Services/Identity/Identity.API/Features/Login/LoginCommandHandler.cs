namespace Identity.API.Features.Login
{
    internal record LoginCommandResult(string Token, string RefreshToken);
    internal record LoginCommand(string Username, string Email, string Password) : IQuery<LoginCommandResult>;
    internal class LoginCommandHandler(UserManager<ApplicationUser> UserManager, ITokenGeneratorService TokenGeneratorService) : IQueryHandler<LoginCommand, LoginCommandResult>
    {
        public async Task<LoginCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken = default)
        {
            var user = (await UserManager.FindByNameAsync(request.Username) ?? await UserManager.FindByEmailAsync(request.Email)) ??
                    throw new NotFoundException($"{request.Username ?? request.Email} not found");

            if (await UserManager.CheckPasswordAsync(user, request.Password))
            {
                var roles = await UserManager.GetRolesAsync(user);
                var token = TokenGeneratorService.GenerateToken(user.Id, user.UserName, user.Email, roles);

                return new LoginCommandResult(token, string.Empty);
            }

            throw new UnauthorizedException("Invalid password");
        }
    }
}
