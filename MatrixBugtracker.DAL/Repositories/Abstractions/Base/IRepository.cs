using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.Domain.Entities.Base;

namespace MatrixBugtracker.DAL.Repositories.Abstractions.Base
{
    public interface IRepository<T> : IRepositoryBase where T : BaseEntity
    {
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<PaginationResult<T>> GetPageAsync(int number, int size);
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetByIdsAsync(IEnumerable<int> ids);
        Task<bool> HasEntityAsync(int id);
    }
}
