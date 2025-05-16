using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(BugtrackerContext db) : base(db) { }

        public async Task<bool> HasEntityAsync(string name)
        {
            return await _dbSet.Where(p => p.Name == name).CountAsync() > 0;
        }
    }
}
