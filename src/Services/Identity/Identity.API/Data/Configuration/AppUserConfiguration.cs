namespace Identity.API.Data.Configuration
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Ignore(e => e.DomainEvents);
        }
    }
}
