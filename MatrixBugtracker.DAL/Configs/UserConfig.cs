using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class UserConfig : DeleteConfig<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

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

            builder.Property(e => e.PhotoFileId).HasColumnName("photo_file_id");

            builder.HasOne(d => d.PhotoFile).WithMany(p => p.Users)
                .HasForeignKey(d => d.PhotoFileId)
                .HasConstraintName("FK_UserPhoto");

            //builder.HasMany(d => d.UserRoles).WithOne();

            base.Configure(builder);
        }
    }
}
