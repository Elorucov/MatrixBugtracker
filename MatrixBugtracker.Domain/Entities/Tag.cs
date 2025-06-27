using MatrixBugtracker.Domain.Entities.Base;

namespace MatrixBugtracker.Domain.Entities;

public partial class Tag : BaseEntity
{
    public string Name { get; set; }

    public bool IsArchived { get; set; }

    public virtual User Creator { get; set; }

    public virtual ICollection<ReportTag> Reports { get; set; } = new List<ReportTag>();
}
