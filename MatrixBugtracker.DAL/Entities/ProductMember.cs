using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.DAL.Entities;

public class ProductMember
{
    public int ProductId { get; set; }
    public int MemberId { get; set; }
    public ProductMemberStatus Status { get; set; }

    public virtual User Member { get; set; }
    public virtual Product Product { get; set; }
}
