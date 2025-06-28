using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Extensions;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace MatrixBugtracker.DAL.Repositories.Implementations.Base
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly BugtrackerContext _db;
        protected readonly DbSet<T> _dbSet;

        public Repository(BugtrackerContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _db.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _db.Remove(entity);
        }

        public async Task<PaginationResult<T>> GetPageAsync(int number, int size)
        {
            return await _dbSet.AsQueryable().GetPageAsync(number, size);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> HasEntityAsync(int id)
        {
            return await _dbSet.FindAsync(id) != null;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
