using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.DTOs.Notifications
{
    public class PlatformNotificationDTO : NotificationDTO
    {
        public PlatformNotificationKind Kind { get; set; }
    }
}
