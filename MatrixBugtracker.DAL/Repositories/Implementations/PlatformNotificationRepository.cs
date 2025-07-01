using MatrixBugtracker.DAL.Data;
using MatrixBugtracker.DAL.Extensions;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using MatrixBugtracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatrixBugtracker.DAL.Repositories.Implementations
{
    public class PlatformNotificationRepository : Repository<PlatformNotification>, IPlatformNotificationRepository
    {
        public PlatformNotificationRepository(BugtrackerContext db) : base(db) { }

        public async Task<PaginationResult<PlatformNotification>> GetWithReadUsersAsync(int number, int size)
        {
            return await _dbSet.Include(n => n.UsersThatRead)
                .OrderByDescending(n => n.Id).GetPageAsync(number, size);
        }

        public async Task<List<int>> GetReadNotificationIdsForUserAsync(int userId)
        {
            return await _db.PlatformNotificationReadUsers.Where(n => n.UserId == userId).Select(n => n.NotificationId).ToListAsync();
        }

        public async Task<List<int>> GetAllNotificationIdsAsync()
        {
            return await _dbSet.Select(n => n.Id).ToListAsync();
        }

        public async Task MarkAsReadAsync(int userId, int notificationId)
        {
            await _db.PlatformNotificationReadUsers.AddAsync(new PlatformNotificationUser
            {
                UserId = userId,
                NotificationId = notificationId
            });
        }
    }
}
