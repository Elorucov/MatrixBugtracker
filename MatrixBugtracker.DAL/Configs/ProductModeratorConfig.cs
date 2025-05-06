using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class ProductModeratorConfig : IEntityTypeConfiguration<ProductModerator>
    {
        public void Configure(EntityTypeBuilder<ProductModerator> builder)
        {
            builder.ToTable("product_moderators");

            builder.HasKey(e => new { e.ProductId, e.ModeratorId }).HasName("K_ProductModerator");
            builder.HasIndex(e => new { e.ProductId, e.ModeratorId }, "UQ_ProductModer").IsUnique();

            builder.Property(e => e.ModeratorId).HasColumnName("moderator_id");
            builder.Property(e => e.ProductId).HasColumnName("product_id");

            builder.HasOne(d => d.Moderator).WithMany()
                .HasForeignKey(d => d.ModeratorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PM_Moderator");

            builder.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PM_Product");
        }
    }
}
