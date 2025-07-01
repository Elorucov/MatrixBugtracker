using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IPlatformNotificationRepository : IRepository<PlatformNotification>
    {
        Task<PaginationResult<PlatformNotification>> GetWithReadUsersAsync(int number, int size);
        Task<List<int>> GetReadNotificationIdsForUserAsync(int userId);
        Task<List<int>> GetAllNotificationIdsAsync();
        Task MarkAsReadAsync(int userId, int notificationId);
    }
}
