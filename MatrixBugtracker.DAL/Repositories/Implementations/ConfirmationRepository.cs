using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class ConfirmationRepository : Repository<Confirmation>, IConfirmationRepository
    {
        public ConfirmationRepository(BugtrackerContext db) : base(db) { }

        public async Task<Confirmation> GetByUserIdAsync(int userId, EmailConfirmationKind kind)
        {
            return await _dbSet.Include(c => c.User)
                .Where(c => c.User.Id == userId && c.Kind == kind).SingleOrDefaultAsync();
        }
    }
}
