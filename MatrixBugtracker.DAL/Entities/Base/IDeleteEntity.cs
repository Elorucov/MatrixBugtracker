namespace MatrixBugtracker.DAL.Entities.Base
{
    public interface IDeleteEntity : IEntity
    {
        public bool IsDeleted { get; set; }
        public int DeletedByUserId { get; set; }
        public DateTime DeletionTime { get; set; }
    }
}
