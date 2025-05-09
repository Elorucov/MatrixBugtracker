using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IUserService
    {
        Task<ResponseDTO<TokenDTO>> LoginAsync(LoginRequestDTO request);
    }
}
