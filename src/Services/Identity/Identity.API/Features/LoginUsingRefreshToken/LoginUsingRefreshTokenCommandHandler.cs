namespace Identity.API.Features.LoginUsingRefreshToken
{
    internal record LoginUsingRefreshTokenCommandResult(string Token, string RefreshToken);
    internal record LoginUsingRefreshTokenCommand(string RefreshToken) : ICommand<LoginUsingRefreshTokenCommandResult>;

    internal class LoginUsingRefreshTokenCommandHandler(UserManager<AppUser> UserManager, IRefreshTokenRepository RefreshTokenRepository, ITokenGeneratorService TokenGeneratorService)
        : ICommandHandler<LoginUsingRefreshTokenCommand, LoginUsingRefreshTokenCommandResult>
    {
        public async Task<LoginUsingRefreshTokenCommandResult> Handle(LoginUsingRefreshTokenCommand request, CancellationToken cancellationToken = default)
        {
            var refreshToken = await RefreshTokenRepository.FindRefreshTokenAsync(request.RefreshToken, cancellationToken);

            if (refreshToken is null || refreshToken.ExpirationDate < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }
            if(refreshToken.User is null)
            {
                throw new UnauthorizedAccessException("User not found for the provided refresh token.");
            }

            var userRoles = await UserManager.GetRolesAsync(refreshToken.User);
            var newToken = TokenGeneratorService.GenerateToken(refreshToken.User.Id, refreshToken.User.UserName, refreshToken.User.Email, userRoles);
            var newRefreshToken = TokenGeneratorService.GenerateRefreshToken();
            await RefreshTokenRepository.OverwriteRefreshTokenAsync(refreshToken, refreshToken.User, newRefreshToken, cancellationToken);

            return new LoginUsingRefreshTokenCommandResult(newToken, newRefreshToken);
        }
    }
}
