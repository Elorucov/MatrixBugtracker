using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class PlatformNotificationConfig : BaseEntityConfig<PlatformNotification>
    {
        public override void Configure(EntityTypeBuilder<PlatformNotification> builder)
        {
            builder.ToTable("platform_notifications");

            builder.Property(e => e.Kind)
                .IsRequired()
                .HasConversion<byte>().HasColumnName("kind");

            builder.Property(e => e.Text).IsRequired().HasMaxLength(1024).HasColumnName("text");

            base.Configure(builder);
        }
    }
}
