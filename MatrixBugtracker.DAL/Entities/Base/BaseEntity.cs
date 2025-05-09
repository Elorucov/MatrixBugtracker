namespace MatrixBugtracker.DAL.Entities.Base
{
    public abstract class BaseEntity : ICreateEntity
    {
        public int Id { get; init; }
        public bool IsDeleted { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
        public int DeletedByUserId { get; set; }
    }
}
