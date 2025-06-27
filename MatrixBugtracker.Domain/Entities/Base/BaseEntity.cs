namespace MatrixBugtracker.Domain.Entities.Base
{
    public abstract class BaseEntity : IDeleteEntity
    {
        public int Id { get; init; }
        public bool IsDeleted { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
