using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
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
    }
}
