using MatrixBugtracker.DAL.Configs.Base;
using MatrixBugtracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatrixBugtracker.DAL.Configs
{
    internal class UserNotificationConfig : BaseEntityConfig<UserNotification>
    {
        public override void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.ToTable("user_notifications");

            builder.Property(e => e.Kind)
                .IsRequired()
                .HasConversion<byte>().HasColumnName("kind");

            builder.Property(e => e.TargetUserId).IsRequired().HasColumnName("target_user_id");

            builder.Property(e => e.LinkedEntityType)
                .HasConversion<byte>().HasColumnName("linked_entity_type");

            builder.Property(e => e.LinkedEntityId).HasColumnName("linked_entity_id");

            builder.Property(e => e.ViewedByTargetUser).IsRequired().HasColumnName("viewed");

            builder.HasOne(d => d.TargetUser)
                .WithMany(p => p.Notifications)
                .HasForeignKey(d => d.TargetUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Notifications_TargetUser");

            base.Configure(builder);
        }
    }
}
