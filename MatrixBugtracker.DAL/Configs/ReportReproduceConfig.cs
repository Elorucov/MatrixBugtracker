using MatrixBugtracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class ReportReproduceConfig : IEntityTypeConfiguration<ReportReproduce>
    {
        public void Configure(EntityTypeBuilder<ReportReproduce> builder)
        {
            builder.ToTable("report_reproduces");

            builder.HasKey(e => new { e.ReportId, e.UserId }).HasName("K_ReportReproduce");
            builder.HasIndex(e => new { e.ReportId, e.UserId }, "UQ_ReportReproduce").IsUnique();

            builder.Property(e => e.ReportId).HasColumnName("report_id");
            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.HasOne(d => d.Report).WithMany(d => d.Reproduces)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RR_Report");

            builder.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RR_User");
        }
    }
}
