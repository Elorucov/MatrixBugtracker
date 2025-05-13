using MatrixBugtracker.DAL.Entities.Base;
using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.DAL.Entities;

public class Report : BaseEntity
{
    public int ProductId { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string Title { get; set; }
    public string Steps { get; set; }
    public string Actual { get; set; }
    public string Supposed { get; set; }
    public ReportSeverity Severity { get; set; }
    public ReportProblemType ProblemType { get; set; }
    public ReportStatus Status { get; set; }
    public bool IsFilesPrivate { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual User Creator { get; set; }
    public virtual Product Product { get; set; }
}
