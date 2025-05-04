using MatrixBugtracker.DAL.Entities.Base;

namespace MatrixBugtracker.DAL.Entities;

public partial class UploadedFile : BaseEntity
{
    public string OriginalName { get; set; }

    public string Path { get; set; }

    public string MimeType { get; set; }

    public virtual User Creator { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
