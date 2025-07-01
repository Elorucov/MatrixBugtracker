using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.DTOs.Notifications
{
    public class UserNotificationDTO : NotificationDTO
    {
        public UserNotificationKind Kind { get; set; }
        public LinkedEntityType LinkedEntityType { get; set; }
        public int LinkedEntityId { get; set; }
    }
}
