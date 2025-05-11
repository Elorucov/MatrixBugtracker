using MatrixBugtracker.DAL.Entities.Base;

namespace MatrixBugtracker.DAL.Entities;

public partial class UploadedFile : BaseEntity
{
    public string OriginalName { get; set; }
    public string Path { get; set; }
    public string MimeType { get; set; }
    public long Length { get; set; }

    public virtual User Creator { get; set; }
    public virtual User PhotoUser { get; set; } // an user that set this file as own photo
    public virtual Product PhotoProduct { get; set; } // a product that set this file as logo
}
