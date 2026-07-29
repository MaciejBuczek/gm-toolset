namespace Identity.API.Data.Configuration
{
    internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Token).IsRequired().HasMaxLength(256);
            builder.Property(r => r.ExpirationDate).IsRequired();
            builder.HasOne(r => r.User)
                   .WithMany()
                   .HasForeignKey(r => r.UserId)
                   .IsRequired();
            builder.HasIndex(r => r.Token).IsUnique();
        }
    }
}
