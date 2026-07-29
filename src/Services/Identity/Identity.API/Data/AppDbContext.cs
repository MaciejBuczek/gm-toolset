namespace Identity.API.Data
{
    public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
        : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options)
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(Constants.DefaultSchema);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(builder);
        }
    }
}
