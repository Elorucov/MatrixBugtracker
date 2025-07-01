using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IUserNotificationRepository : IRepository<UserNotification>
    {
        Task<PaginationResult<UserNotification>> GetForUserAsync(int userId, int number, int size);
    }
}
