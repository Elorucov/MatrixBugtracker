using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class RoleConfig : EntityConfig<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");
            builder.HasIndex(e => e.Name, "UQ_RoleName").IsUnique();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnName("name");

            //builder.HasOne(d => d.Creator).WithMany(p => p.Roles)
            //    .HasForeignKey(d => d.CreatorId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_Roles_Creator");

            List<Role> buildInRoles = new List<Role>();

            foreach (var role in Enum.GetValues<RoleEnum>())
            {
                buildInRoles.Add(new Role
                {
                    Id = (int)role,
                    Name = role.ToString()
                });
            }

            builder.HasData(buildInRoles);

            base.Configure(builder);
        }
    }
}
