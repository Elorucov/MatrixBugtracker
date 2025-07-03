using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdWithPhotoAsync(int id);
        Task<User> GetByIdWithProductsAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<List<KeyValuePair<string, string>>> GetEmailsAsync(IEnumerable<int> ids);
        Task<int> GetUsersCountWithModeratorNamesAsync();
    }
}
