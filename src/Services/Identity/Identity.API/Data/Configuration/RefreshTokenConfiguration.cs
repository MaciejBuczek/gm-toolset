namespace Identity.API.Data.Configuration
{
    internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Token).IsRequired().HasMaxLength(128);
            builder.Property(r => r.ExpirationDate).IsRequired();
            builder.Property(r => r.IsExpired).IsRequired().HasDefaultValue(false);
            builder.HasOne(r => r.User)
                   .WithMany()
                   .HasForeignKey(r => r.UserId);
            builder.HasIndex(r => r.Token).IsUnique();
        }
    }
}