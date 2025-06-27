using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;

namespace MatrixBugtracker.DAL.Repositories.Abstractions
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken> GetByTokenAsync(string refreshToken, int userId);
    }
}
