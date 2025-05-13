using MatrixBugtracker.DAL.Entities.Base;
using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.DAL.Entities;

public partial class User : IDeleteEntity
{
    public int Id { get; init; }
    public bool IsDeleted { get; set; }
    public int DeletedByUserId { get; set; }
    public DateTime DeletionTime { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public string? ModeratorName { get; set; }
    public int? PhotoFileId { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<UploadedFile> UploadedFiles { get; set; } = new List<UploadedFile>();
    public virtual UploadedFile PhotoFile { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
