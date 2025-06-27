using MatrixBugtracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class PlatformNotificationUserConfig : IEntityTypeConfiguration<PlatformNotificationUser>
    {
        public void Configure(EntityTypeBuilder<PlatformNotificationUser> builder)
        {
            builder.ToTable("platform_notification_read_users");

            builder.HasKey(e => new { e.UserId, e.NotificationId }).HasName("K_PlatNotifUser");
            builder.HasIndex(e => new { e.UserId, e.NotificationId }, "UQ_PlatNotifUser").IsUnique();

            builder.Property(e => e.NotificationId).HasColumnName("notification_id");
            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.HasOne(d => d.User).WithMany(u => u.PlatformNotifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PNU_User");

            builder.HasOne(d => d.Notification).WithMany(p => p.UsersThatRead)
                .HasForeignKey(d => d.NotificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PNU_Notification");
        }
    }
}
