using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Extensions;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using MatrixBugtracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class UserNotificationRepository : Repository<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(BugtrackerContext db) : base(db) { }

        public async Task<PaginationResult<UserNotification>> GetForUserAsync(int userId, int number, int size)
        {
            return await _dbSet.Where(n => n.TargetUserId == userId).OrderByDescending(n => n.Id).GetPageAsync(number, size);
        }

        public async Task<List<UserNotification>> GetForUserUnreadAsync(int userId)
        {
            return await _dbSet.Where(n => n.TargetUserId == userId && !n.ViewedByTargetUser).ToListAsync();
        }
    }
}
