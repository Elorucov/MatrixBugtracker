using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Extensions;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;
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

        public async Task<User> GetByIdWithPhotoAsync(int id)
        {
            return await _dbSet.Include(e => e.PhotoFile).SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByIdWithProductsAsync(int id)
        {
            return await _dbSet.Include(e => e.CreatedProducts).SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<KeyValuePair<string, string>>> GetEmailsAsync(IEnumerable<int> ids)
        {
            return await _dbSet.Where(u => ids.Contains(u.Id)).Select(u => new KeyValuePair<string, string>(u.Email, u.FirstName)).ToListAsync();
        }

        public async Task<int> GetUsersCountWithModeratorNamesAsync()
        {
            return await _dbSet.Where(e => e.ModeratorName != null).CountAsync();
        }

        public async Task<PaginationResult<User>> GetByRoleAsync(UserRole role, int number, int size)
        {
            return await _dbSet.Include(e => e.PhotoFile)
                .Where(e => e.Role == role).GetPageAsync(number, size);
        }

        public async Task<PaginationResult<User>> SearchAsync(string name, int number, int size)
        {
            return await _dbSet.Include(e => e.PhotoFile)
                .Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name)).GetPageAsync(number, size);
        }
    }
}
