﻿using MatrixBugtracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class ReportTagConfig : IEntityTypeConfiguration<ReportTag>
    {
        public void Configure(EntityTypeBuilder<ReportTag> builder)
        {
            builder.ToTable("report_tags");

            builder.HasKey(e => new { e.ReportId, e.TagId }).HasName("K_ReportTag");
            builder.HasIndex(e => new { e.ReportId, e.TagId }, "UQ_ReportTag").IsUnique();

            builder.Property(e => e.ReportId).HasColumnName("report_id");
            builder.Property(e => e.TagId).HasColumnName("tag_id");

            builder.HasOne(d => d.Report).WithMany(r => r.Tags)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RT_Report");

            builder.HasOne(d => d.Tag).WithMany(t => t.Reports)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RT_Tag");
        }
    }
}
