namespace MatrixBugtracker.DAL.Entities.Base
{
    public interface ICreateEntity : IDeleteEntity
    {
        public int CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
