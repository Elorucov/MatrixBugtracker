using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class CommentConfig : BaseEntityConfig<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comments");

            builder.Property(e => e.AsModerator).IsRequired().HasColumnName("as_moderator");
            builder.Property(e => e.IsAttachmentsPrivate).IsRequired().HasColumnName("is_attachments_private");
            builder.Property(e => e.NewSeverity).HasColumnName("new_severity");
            builder.Property(e => e.NewStatus).HasColumnName("new_status");
            builder.Property(e => e.ReportId).IsRequired().HasColumnName("report_id");
            builder.Property(e => e.Text).HasMaxLength(2048).HasColumnName("text");
            builder.Property(e => e.UpdateTime).HasColumnName("update_time");

            builder.HasOne(d => d.Creator)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Comment_Creator");

            builder.HasOne(d => d.Report)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Comment_Report");
            base.Configure(builder);
        }
    }
}
