namespace Identity.API.Services.Contracts
{
    internal interface IRefreshTokenRepository
    {
        Task SaveRefreshTokenToDbAsync(AppUser user, string refreshToken, CancellationToken cancellationToken = default);
        Task<RefreshToken?> FindRefreshTokenAsync(string token, CancellationToken cancellationToken = default);
        Task OverwriteRefreshTokenAsync(RefreshToken oldToken, AppUser user, string newRefreshToken, CancellationToken cancellationToken = default);
    }
}
