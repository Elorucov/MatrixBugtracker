using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class ReportReproduceConfig : IEntityTypeConfiguration<ReportReproduce>
    {
        public void Configure(EntityTypeBuilder<ReportReproduce> builder)
        {
            builder.HasNoKey().ToTable("report_reproduces");

            builder.HasIndex(e => new { e.ReportId, e.UserId }, "UQ_ReportReproduces").IsUnique();

            builder.Property(e => e.ReportId).HasColumnName("report_id");
            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.HasOne(d => d.Report).WithMany()
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
