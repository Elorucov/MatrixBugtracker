using MatrixBugtracker.Domain.Entities.Base;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.Domain.Entities
{
    public class PlatformNotification : BaseEntity
    {
        public PlatformNotificationKind Kind { get; set; }
        public string Text { get; set; }

        public virtual ICollection<PlatformNotificationUser> UsersThatRead { get; set; } = new List<PlatformNotificationUser>();
    }
}
