using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            builder.HasIndex(e => e.UserId, "UQ_RTUserId").IsUnique();
            builder.HasIndex(e => e.Token, "UQ_RefreshToken").IsUnique();

            builder.HasOne(d => d.User)
                .WithOne(p => p.RefreshToken)
                .HasForeignKey<RefreshToken>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RT_UserId");
        }
    }
}
