namespace Identity.API.Services
{
    internal class RefreshTokenRepository(AppDbContext DbContext, IOptions<Jwt> ConfigurationOptions) : IRefreshTokenRepository
    {
        public async Task<RefreshToken?> FindRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await DbContext.RefreshTokens.Include(r => r.User)
                            .FirstOrDefaultAsync(r => r.Token == token, cancellationToken);
        }

        public async Task OverwriteRefreshTokenAsync(RefreshToken oldToken, AppUser user, string newRefreshToken, CancellationToken cancellationToken = default)
        {
            using var transaction = DbContext.Database.BeginTransaction();

            oldToken.IsExpired = true;
            DbContext.RefreshTokens.Update(oldToken);
            await SaveRefreshTokenToDbAsync(user, newRefreshToken, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }

        public async Task SaveRefreshTokenToDbAsync(AppUser user, string refreshToken, CancellationToken cancellationToken = default)
        {
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpirationDate = DateTime.UtcNow.AddDays(ConfigurationOptions.Value.RefreshTokenExpirationInDays)
            };
            await DbContext.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
