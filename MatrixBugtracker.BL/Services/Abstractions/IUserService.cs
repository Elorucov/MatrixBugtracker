using MatrixBugtracker.BL.DTOs.Admin;
using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IUserService
    {
        Task<User> GetSingleUserAsync(int userId);
        Task<ResponseDTO<TokenDTO>> LoginAsync(LoginRequestDTO request);
        Task<ResponseDTO<TokenDTO>> RefreshAsync(int userId, string refreshToken);
        Task<ResponseDTO<bool>> CreateUserAsync(RegisterRequestDTO request);
        Task<ResponseDTO<UserDTO>> GetByIdAsync(int userId);
        Task<ResponseDTO<bool>> SetUserRoleAsync(SetRoleRequestDTO request);
        Task<ResponseDTO<bool>> EditAsync(UserEditDTO request);
        Task<ResponseDTO<bool>> ChangePhotoAsync(int photoFileId);
    }
}
