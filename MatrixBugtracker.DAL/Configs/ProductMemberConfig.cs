using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class ProductMemberConfig : IEntityTypeConfiguration<ProductMember>
    {
        public void Configure(EntityTypeBuilder<ProductMember> builder)
        {
            builder.ToTable("product_members");

            builder.HasKey(e => new { e.ProductId, e.MemberId }).HasName("K_ProductMember");
            builder.HasIndex(e => new { e.ProductId, e.MemberId }, "UQ_ProductMember").IsUnique();

            builder.Property(e => e.MemberId).HasColumnName("member_id");
            builder.Property(e => e.ProductId).HasColumnName("product_id");
            builder.Property(e => e.Status).HasColumnName("status");

            builder.HasOne(d => d.Member).WithMany(u => u.JoinedProducts)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PU_Member");

            builder.HasOne(d => d.Product).WithMany(p => p.ProductMembers)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PU_Product");
        }
    }
}
