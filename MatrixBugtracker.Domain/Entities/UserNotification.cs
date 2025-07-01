using MatrixBugtracker.Domain.Entities.Base;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.Domain.Entities
{
    public class UserNotification : BaseEntity
    {
        public UserNotificationKind Kind { get; set; }
        public int TargetUserId { get; set; }
        public string Text { get; set; }
        public LinkedEntityType? LinkedEntityType { get; set; }
        public int? LinkedEntityId { get; set; }
        public bool ViewedByTargetUser { get; set; }

        public virtual User TargetUser { get; set; }
    }
}
