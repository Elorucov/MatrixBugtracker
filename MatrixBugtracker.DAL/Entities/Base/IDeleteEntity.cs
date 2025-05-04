namespace MatrixBugtracker.DAL.Entities.Base
{
    public interface IDeleteEntity : IEntity
    {
        public bool IsDeleted { get; set; }
    }
}
