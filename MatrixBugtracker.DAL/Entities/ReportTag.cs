namespace MatrixBugtracker.DAL.Entities;

public class ReportTag
{
    public int ReportId { get; set; }

    public int TagId { get; set; }

    public virtual Report Report { get; set; }

    public virtual Tag Tag { get; set; }
}
