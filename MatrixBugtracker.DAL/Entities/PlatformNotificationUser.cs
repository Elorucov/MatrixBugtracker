namespace MatrixBugtracker.DAL.Entities
{
    public class PlatformNotificationUser
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }

        public virtual PlatformNotification Notification { get; set; }
        public virtual User User { get; set; }
    }
}
