using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdWithPhotoAsync(int id);
        Task<User> GetByIdWithProductsAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<List<KeyValuePair<string, string>>> GetEmailsAsync(IEnumerable<int> ids);
        Task<int> GetUsersCountWithModeratorNamesAsync();
        Task<PaginationResult<User>> GetByRoleAsync(UserRole role, int number, int size);
        Task<PaginationResult<User>> SearchAsync(string name, int number, int size);
    }
}
