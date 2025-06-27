using MatrixBugtracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class ReportAttachmentConfig : IEntityTypeConfiguration<ReportAttachment>
    {
        public void Configure(EntityTypeBuilder<ReportAttachment> builder)
        {
            builder.ToTable("report_attachments");

            builder.HasKey(e => new { e.ReportId, e.FileId }).HasName("K_ReportAttachment");
            builder.HasIndex(e => new { e.ReportId, e.FileId }, "UQ_ReportAttachment").IsUnique();

            builder.Property(e => e.FileId).HasColumnName("file_id");
            builder.Property(e => e.ReportId).HasColumnName("report_id");

            builder.HasOne(d => d.File).WithMany()
                .HasForeignKey(d => d.FileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RA_Attachment");

            builder.HasOne(d => d.Report).WithMany(r => r.Attachments)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RA_Report");
        }
    }
}
