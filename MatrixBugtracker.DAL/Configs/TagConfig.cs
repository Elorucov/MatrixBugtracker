using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class TagConfig : BaseEntityConfig<Tag>
    {
        public override void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("tags");
            builder.HasIndex(e => e.Name, "UQ_TagName").IsUnique();

            builder.Property(e => e.IsArchived).HasColumnName("is_archived");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnName("name");

            builder.HasOne(d => d.Creator).WithMany(p => p.Tags)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tag_Creator");

            base.Configure(builder);
        }
    }
}
