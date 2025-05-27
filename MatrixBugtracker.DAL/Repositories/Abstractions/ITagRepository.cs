using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<List<Tag>> GetIntersectingAsync(string[] tags);
        Task<List<Tag>> GetAllAsync();
        Task<List<Tag>> GetUnarchivedAsync();
        Task AddBatchAsync(string[] tags);
    }
}
