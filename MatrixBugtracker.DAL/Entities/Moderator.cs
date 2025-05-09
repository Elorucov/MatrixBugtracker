using MatrixBugtracker.DAL.Entities.Base;

namespace MatrixBugtracker.DAL.Entities;

public class Moderator : IDeleteEntity
{
    public int Id { get; init; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public bool IsDeleted { get; set; }
    public int DeletedByUserId { get; set; }
    public DateTime DeletionTime { get; set; }
}
