using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class UserConfig : DeleteConfig<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.Property(x => x.IsEmailConfirmed).IsRequired().HasColumnName("is_email_confirmed");

            builder.HasIndex(e => e.Email, "UQ_Email").IsUnique();

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("email");

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnName("first_name");

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnName("last_name");

            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("password");

            builder.Property(e => e.Role)
                .IsRequired()
                .HasConversion<byte>()
                .HasColumnName("role");

            builder.Property(e => e.ModeratorName)
                .HasMaxLength(32)
                .HasColumnName("moderator_name");

            builder.Property(e => e.PhotoFileId).HasColumnName("photo_file_id");

            builder.HasOne(d => d.PhotoFile).WithOne(p => p.PhotoUser)
                .HasForeignKey<User>(d => d.PhotoFileId)
                .HasConstraintName("FK_UserPhoto");

            base.Configure(builder);
        }
    }
}
