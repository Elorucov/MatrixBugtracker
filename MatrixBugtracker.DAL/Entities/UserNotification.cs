using MatrixBugtracker.DAL.Entities.Base;
using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.DAL.Entities
{
    public class UserNotification : BaseEntity
    {
        public UserNotificationKind Kind { get; set; }
        public int TargetUserId { get; set; }
        public LinkedEntityType? LinkedEntityType { get; set; }
        public int? LinkedEntityId { get; set; }
        public bool ViewedByTargetUser { get; set; }

        public virtual User TargetUser { get; set; }
    }
}
