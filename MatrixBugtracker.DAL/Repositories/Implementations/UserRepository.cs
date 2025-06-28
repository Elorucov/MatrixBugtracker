using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using MatrixBugtracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BugtrackerContext db) : base(db) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbSet.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdWithIncludeAsync(int id)
        {
            return await _dbSet.Include(e => e.PhotoFile).SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<int> GetUsersCountWithModeratorNamesAsync()
        {
            return await _dbSet.Where(e => e.ModeratorName != null).CountAsync();
        }
    }
}
