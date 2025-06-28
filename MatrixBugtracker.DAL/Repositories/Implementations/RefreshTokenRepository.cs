using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using MatrixBugtracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(BugtrackerContext db) : base(db) { }

        public async Task<RefreshToken> GetByTokenAsync(string refreshToken, int userId)
        {
            return await _dbSet.Include(e => e.User)
                .Where(e => e.Token == refreshToken && e.UserId == userId)
                .SingleOrDefaultAsync();
        }
    }
}
