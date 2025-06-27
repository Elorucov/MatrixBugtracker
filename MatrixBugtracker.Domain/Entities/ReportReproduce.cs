namespace MatrixBugtracker.Domain.Entities;

public class ReportReproduce
{
    public int ReportId { get; set; }
    public int UserId { get; set; }

    public virtual Report Report { get; set; }
    public virtual User User { get; set; }
}
