using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class ReportConfig : BaseEntityConfig<Report>
    {
        public override void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("reports");

            builder.Property(e => e.Actual).IsRequired().HasColumnType("text").HasColumnName("actual");
            builder.Property(e => e.IsFilesPrivate).IsRequired().HasColumnName("is_files_private");

            builder.Property(e => e.ProblemType)
                .IsRequired()
                .HasConversion<byte>()
                .HasColumnName("problem_type");

            builder.Property(e => e.ProductId).IsRequired().HasColumnName("product_id");

            builder.Property(e => e.Severity)
                .IsRequired()
                .HasConversion<byte>()
                .HasColumnName("severity");

            builder.Property(e => e.Status).IsRequired()
                .HasConversion<byte>()
                .HasColumnName("status");

            builder.Property(e => e.Steps).IsRequired().HasColumnType("text").HasColumnName("steps");
            builder.Property(e => e.Supposed).IsRequired().HasColumnType("text").HasColumnName("supposed");
            builder.Property(e => e.Title).IsRequired().HasMaxLength(128).HasColumnName("title");
            builder.Property(e => e.UpdateTime).HasColumnName("update_time");

            builder.HasOne(d => d.Creator).WithMany(p => p.Reports)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Report_Creator");

            builder.HasOne(d => d.Product).WithMany(p => p.Reports)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_Product");

            base.Configure(builder);
        }
    }
}
