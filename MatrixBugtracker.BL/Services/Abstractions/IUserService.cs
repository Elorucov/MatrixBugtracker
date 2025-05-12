using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IUserService
    {
        Task<ResponseDTO<TokenDTO>> LoginAsync(LoginRequestDTO request);
        Task<ResponseDTO<bool>> CreateUserAsync(RegisterRequestDTO request);
        Task<ResponseDTO<UserDTO>> GetByIdAsync(int userId);
        Task<UserRole?> GetUserRoleAsync(int userId);
        Task<ResponseDTO<bool>> EditAsync(UserEditDTO request);
    }
}
