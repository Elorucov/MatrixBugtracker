using MatrixBugtracker.Domain.Entities.Base;
using MatrixBugtracker.DAL.Models;

namespace MatrixBugtracker.DAL.Repositories.Abstractions.Base
{
    public interface IRepository<T> : IRepositoryBase where T : IEntity
    {
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<PaginationResult<T>> GetPageAsync(int number, int size);
        Task<T> GetByIdAsync(int id);
        Task<bool> HasEntityAsync(int id);
    }
}
