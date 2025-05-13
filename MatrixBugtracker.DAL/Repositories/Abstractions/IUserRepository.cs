using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdWithIncludeAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<int> GetUsersCountWithModeratorNamesAsync();
    }
}
