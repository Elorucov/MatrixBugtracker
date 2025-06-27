using MatrixBugtracker.Domain.Entities.Base;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.Domain.Entities;

public class Report : BaseEntity
{
    public int ProductId { get; set; }
    public string Title { get; set; }
    public string Steps { get; set; }
    public string Actual { get; set; }
    public string Supposed { get; set; }
    public ReportSeverity Severity { get; set; }
    public ReportProblemType ProblemType { get; set; }
    public ReportStatus Status { get; set; }
    public bool IsAttachmentsPrivate { get; set; }
    public bool IsSeveritySetByModerator { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<ReportAttachment> Attachments { get; set; } = new List<ReportAttachment>();
    public virtual ICollection<ReportTag> Tags { get; set; } = new List<ReportTag>();
    public virtual ICollection<ReportReproduce> Reproduces { get; set; } = new List<ReportReproduce>();
    public virtual User Creator { get; set; }
    public virtual Product Product { get; set; }
}
