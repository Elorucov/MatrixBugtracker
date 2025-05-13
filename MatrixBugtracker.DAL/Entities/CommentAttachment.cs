namespace MatrixBugtracker.DAL.Entities;

public class CommentAttachment
{
    public int CommentId { get; set; }
    public int FileId { get; set; }

    public virtual Comment Comment { get; set; }
    public virtual UploadedFile File { get; set; }
}
