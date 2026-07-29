namespace Identity.API.Data.Configuration
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.HasData(
                new IdentityRole<Guid>
                {
                    Id = Constants.Roles.AdminId,
                    Name = Constants.Roles.Admin,
                    NormalizedName = Constants.Roles.AdminNormalized
                },
                new IdentityRole<Guid>
                {
                    Id = Constants.Roles.UserId,
                    Name = Constants.Roles.User,
                    NormalizedName = Constants.Roles.UserNormalized
                }
            );
        }
    }
}
