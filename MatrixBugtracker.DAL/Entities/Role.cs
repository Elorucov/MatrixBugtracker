using MatrixBugtracker.DAL.Entities.Base;

namespace MatrixBugtracker.DAL.Entities;

public class Role : IEntity
{
    public int Id { get; init; }

    public string Name { get; set; }
}
