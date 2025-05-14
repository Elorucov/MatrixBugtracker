using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens");

            builder.Property(e => e.UserId).IsRequired().HasColumnName("user_id");
            builder.Property(e => e.ExpirationTime).IsRequired().HasColumnName("expiration_time");
            builder.Property(e => e.Token).IsRequired().HasColumnName("token");

            builder.HasIndex(e => e.Token, "UQ_RefreshToken").IsUnique();

            builder.HasOne(d => d.User)
                .WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RT_UserId");
        }
    }
}
