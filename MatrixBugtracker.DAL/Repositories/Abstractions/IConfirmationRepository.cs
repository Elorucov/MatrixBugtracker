using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IConfirmationRepository : IRepository<Confirmation>
    {
        Task<Confirmation> GetByUserIdAsync(int userId, EmailConfirmationKind kind);
    }
}
