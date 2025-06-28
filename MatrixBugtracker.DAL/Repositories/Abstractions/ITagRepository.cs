using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<List<Tag>> GetIntersectingAsync(string[] tags);
        Task<Tag> GetByNameAsync(string name);
        Task<List<Tag>> GetAllAsync();
        Task<List<Tag>> GetUnarchivedAsync();
        Task AddBatchAsync(string[] tags);
    }
}
