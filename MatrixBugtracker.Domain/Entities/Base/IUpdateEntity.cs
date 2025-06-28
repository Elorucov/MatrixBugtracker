namespace MatrixBugtracker.Domain.Entities.Base
{
    public interface IUpdateEntity : ICreateEntity
    {
        int? UpdateUserId { get; set; }
        DateTime? UpdateTime { get; set; }
    }
}
