using MatrixBugtracker.DAL.Entities.Base;
using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.DAL.Entities
{
    public class Confirmation : BaseEntity
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public EmailConfirmationKind Kind { get; set; }
    }
}
