using MatrixBugtracker.DAL.Entities.Base;
using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.DAL.Entities
{
    public class PlatformNotification : BaseEntity
    {
        public PlatformNotificationKind Kind { get; set; }
        public string Text { get; set; }

        public virtual ICollection<PlatformNotificationUser> UsersThatRead { get; set; } = new List<PlatformNotificationUser>();
    }
}
