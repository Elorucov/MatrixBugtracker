using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class CommentAttachmentConfig : IEntityTypeConfiguration<CommentAttachment>
    {
        public void Configure(EntityTypeBuilder<CommentAttachment> builder)
        {
            builder.HasNoKey().ToTable("comment_attachments");

            builder.HasIndex(e => new { e.CommentId, e.FileId }, "UQ_CommentAttachment").IsUnique();

            builder.Property(e => e.CommentId).IsRequired().HasColumnName("comment_id");
            builder.Property(e => e.FileId).IsRequired().HasColumnName("file_id");

            builder.HasOne(d => d.Comment).WithMany()
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK_CA_Comment");

            builder.HasOne(d => d.File).WithMany()
                .HasForeignKey(d => d.FileId)
                .HasConstraintName("FK_CA_Attachment");
        }
    }
}
