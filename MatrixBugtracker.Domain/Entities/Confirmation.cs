using MatrixBugtracker.Domain.Entities.Base;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.Domain.Entities
{
    public class Confirmation : BaseEntity
    {
        public int UserId { get; set; }
        public string Code { get; set; }
        public EmailConfirmationKind Kind { get; set; }

        public virtual User User { get; set; }
    }
}
