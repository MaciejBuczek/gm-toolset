namespace Identity.API.Features.Login
{
    internal record LoginCommandResult(string Token, string RefreshToken);
    internal record LoginCommand(string Username, string Email, string Password) : IQuery<LoginCommandResult>;
    internal class LoginCommandHandler(UserManager<AppUser> UserManager, IRefreshTokenRepository RefreshTokenRepository, ITokenGeneratorService TokenGeneratorService)
        : IQueryHandler<LoginCommand, LoginCommandResult>
    {
        public async Task<LoginCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken = default)
        {
            var user = (await UserManager.FindByNameAsync(request.Username) ?? await UserManager.FindByEmailAsync(request.Email)) ??
                throw new UnauthorizedException("Invalid user or password");

            if (await UserManager.CheckPasswordAsync(user, request.Password))
            {
                var roles = await UserManager.GetRolesAsync(user);
                var token = TokenGeneratorService.GenerateToken(user.Id, user.UserName, user.Email, roles);
                var refreshToken = TokenGeneratorService.GenerateRefreshToken();
                await RefreshTokenRepository.SaveRefreshTokenToDbAsync(user, refreshToken, cancellationToken);

                return new LoginCommandResult(token, refreshToken);
            }

            throw new UnauthorizedException("Invalid user or password");
        }
    }
}
