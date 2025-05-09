using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class ProductConfig : BaseEntityConfig<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.HasIndex(e => e.Name, "UQ_ProductName").IsUnique();

            builder.Property(e => e.AccessLevel).HasColumnName("access_level");
            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnName("description");

            builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            builder.Property(e => e.IsOver).HasColumnName("is_over");
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnName("name");

            builder.Property(e => e.Type)
                .HasConversion<byte>()
                .HasColumnName("type");

            builder.HasOne(d => d.Creator).WithMany(p => p.Products)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Creator");

            base.Configure(builder);
        }
    }
}
