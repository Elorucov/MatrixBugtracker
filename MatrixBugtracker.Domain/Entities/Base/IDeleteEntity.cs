namespace MatrixBugtracker.Domain.Entities.Base
{
    public interface IDeleteEntity : IUpdateEntity
    {
        public bool IsDeleted { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
