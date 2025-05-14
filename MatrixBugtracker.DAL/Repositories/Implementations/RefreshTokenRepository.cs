using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
