using MatrixBugtracker.Domain.Entities.Base;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.Domain.Entities;

public class Comment : BaseEntity
{
    public int ReportId { get; set; }
    public string Text { get; set; }
    public ReportSeverity? NewSeverity { get; set; }
    public ReportStatus? NewStatus { get; set; }
    public bool IsAttachmentsPrivate { get; set; }
    public bool AsModerator { get; set; }

    public virtual User Creator { get; set; }
    public virtual Report Report { get; set; }
    public virtual ICollection<CommentAttachment> Attachments { get; set; } = new List<CommentAttachment>();
}
