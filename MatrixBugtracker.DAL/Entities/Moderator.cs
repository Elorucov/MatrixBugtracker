using MatrixBugtracker.DAL.Entities.Base;

namespace MatrixBugtracker.DAL.Entities;

public class Moderator : BaseEntity
{
    public int UserId { get; set; }

    public virtual User Creator { get; set; }

    public virtual User User { get; set; }
}
