namespace MatrixBugtracker.DAL.Entities;

public class ReportAttachment
{
    public int ReportId { get; set; }

    public int FileId { get; set; }

    public virtual UploadedFile File { get; set; }

    public virtual Product Report { get; set; }
}
