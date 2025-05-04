namespace MatrixBugtracker.DAL.Entities.Base
{
    public interface ICreateEntity : IEntity
    {
        public int CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
