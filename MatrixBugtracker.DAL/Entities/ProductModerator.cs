namespace MatrixBugtracker.DAL.Entities;

public class ProductModerator
{
    public int ProductId { get; set; }

    public int ModeratorId { get; set; }

    public virtual Moderator Moderator { get; set; }

    public virtual Product Product { get; set; }
}
