using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_roles");

            builder.HasKey(e => new { e.UserId, e.RoleId }).HasName("K_UserRole");
            builder.HasIndex(e => new { e.UserId, e.RoleId }, "UQ_UserRole").IsUnique();

            builder.Property(e => e.RoleId).HasColumnName("role_id");
            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.HasOne(d => d.Role).WithMany()
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UR_Role");

            builder.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UR_User");
        }
    }
}
