using MatrixBugtracker.DAL.Entities.Base;
using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.DAL.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public ProductAccessLevel AccessLevel { get; set; }

    public ProductType Type { get; set; }

    public bool IsOver { get; set; }

    public virtual User Creator { get; set; }

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
