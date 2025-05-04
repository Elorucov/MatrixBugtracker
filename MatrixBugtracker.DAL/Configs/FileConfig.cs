using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class FileConfig : BaseEntityConfig<UploadedFile>
    {
        public override void Configure(EntityTypeBuilder<UploadedFile> builder)
        {
            builder.ToTable("files");

            builder.HasIndex(e => e.Path, "UQ_FilePath").IsUnique();

            builder.Property(e => e.MimeType)
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnName("mime_type");

            builder.Property(e => e.OriginalName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("original_name");

            builder.Property(e => e.Path)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("path");

            builder.HasOne(d => d.Creator).WithMany(p => p.UploadedFiles)
                .HasForeignKey(d => d.CreatorId)
                .HasConstraintName("FK_Files_Creator");

            base.Configure(builder);
        }
    }
}
