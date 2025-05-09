using MatrixBugtracker.BL.DTOs.Auth;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface ITokenService
    {
        TokenDTO GetToken(int userId);
    }
}
