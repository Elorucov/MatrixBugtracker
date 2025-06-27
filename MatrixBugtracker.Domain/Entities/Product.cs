using MatrixBugtracker.Domain.Entities.Base;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int? PhotoFileId { get; set; }
    public ProductAccessLevel AccessLevel { get; set; }
    public ProductType Type { get; set; }
    public bool IsOver { get; set; }

    public virtual User Creator { get; set; }
    public virtual UploadedFile PhotoFile { get; set; }
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<ProductMember> ProductMembers { get; set; } = new List<ProductMember>();
}
