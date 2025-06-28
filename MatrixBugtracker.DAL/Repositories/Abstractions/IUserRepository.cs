using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdWithIncludeAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<int> GetUsersCountWithModeratorNamesAsync();
    }
}
