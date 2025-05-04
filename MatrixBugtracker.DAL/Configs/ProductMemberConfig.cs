using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class ProductMemberConfig : IEntityTypeConfiguration<ProductMember>
    {
        public void Configure(EntityTypeBuilder<ProductMember> builder)
        {
            builder.HasNoKey().ToTable("product_members");

            builder.HasIndex(e => new { e.ProductId, e.MemberId }, "UQ_ProductMember").IsUnique();

            builder.Property(e => e.MemberId).HasColumnName("member_id");
            builder.Property(e => e.ProductId).HasColumnName("product_id");
            builder.Property(e => e.Status).HasColumnName("status");

            builder.HasOne(d => d.Member).WithMany()
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PU_Member");

            builder.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PU_Product");
        }
    }
}
