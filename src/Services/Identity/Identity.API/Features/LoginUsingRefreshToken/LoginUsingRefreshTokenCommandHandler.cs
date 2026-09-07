namespace Identity.API.Features.LoginUsingRefreshToken
{
    public record LoginUsingRefreshTokenCommandResult(string Token, string RefreshToken);
    public record LoginUsingRefreshTokenCommand(string RefreshToken) : ICommand<LoginUsingRefreshTokenCommandResult>;

    public class LoginUsingRefreshTokenCommandHandler(UserManager<AppUser> UserManager, IRefreshTokenRepository RefreshTokenRepository, ITokenGeneratorService TokenGeneratorService)
        : ICommandHandler<LoginUsingRefreshTokenCommand, LoginUsingRefreshTokenCommandResult>
    {
        public async Task<LoginUsingRefreshTokenCommandResult> Handle(LoginUsingRefreshTokenCommand request, CancellationToken cancellationToken = default)
        {
            var refreshToken = await RefreshTokenRepository.FindRefreshTokenAsync(request.RefreshToken, cancellationToken);

            if (refreshToken is null || refreshToken.ExpirationDate < DateTime.UtcNow)
            {
                throw new UnauthorizedException("Invalid or expired refresh token.");
            }
            if(refreshToken.User is null || (await UserManager.FindByIdAsync(refreshToken.User.Id.ToString()) is null))
            {
                throw new UnauthorizedException("User not found for the provided refresh token.");
            }
            var userRoles = await UserManager.GetRolesAsync(refreshToken.User);
            if(!userRoles.Any())
            {
                throw new IdentityRoleNotFoundException("User has no roles assigned.");
            }

            try
            {
                var newToken = TokenGeneratorService.GenerateToken(refreshToken.User.Id, refreshToken.User.UserName, refreshToken.User.Email, userRoles);
                var newRefreshToken = TokenGeneratorService.GenerateRefreshToken();
                await RefreshTokenRepository.OverwriteRefreshTokenAsync(refreshToken, refreshToken.User, newRefreshToken, cancellationToken);

                return new LoginUsingRefreshTokenCommandResult(newToken, newRefreshToken);
            }
            catch(DbUpdateException ex)
            {
                throw new IdentityException(ex.Message);
            }
        }
    }
}
