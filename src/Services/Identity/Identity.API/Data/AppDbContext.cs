namespace Identity.API.Data
{
    public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(Constants.DefaultSchema);

            base.OnModelCreating(builder);
        }
    }
}
