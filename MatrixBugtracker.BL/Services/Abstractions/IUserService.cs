using MatrixBugtracker.BL.DTOs.Admin;
using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.Domain.Entities;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IUserService
    {
        Task<User> GetSingleUserAsync(int userId);
        Task<List<KeyValuePair<string, string>>> GetNamesAndEmailsAsync(IEnumerable<int> userIds);
        Task<ResponseDTO<TokenDTO>> LoginAsync(LoginRequestDTO request);
        Task<ResponseDTO<TokenDTO>> RefreshAsync(int userId, string refreshToken);
        Task<ResponseDTO<bool>> CreateUserAsync(RegisterRequestDTO request);
        Task<ResponseDTO<bool>> SendPasswordResetConfirmationAsync(string email);
        Task<ResponseDTO<bool>> ResetPasswordAsync(PasswordResetRequestDTO request);
        Task<ResponseDTO<UserDTO>> GetByIdAsync(int userId);
        Task<ResponseDTO<PageDTO<UserDTO>>> GetUsersByRoleAsync(GetUsersByRoleRequestDTO request);
        Task<ResponseDTO<PageDTO<UserDTO>>> SearchUsersAsync(PaginatedSearchRequestDTO request);
        Task<ResponseDTO<bool>> SetUserRoleAsync(SetRoleRequestDTO request);
        Task<ResponseDTO<bool>> EditAsync(UserEditDTO request);
        Task<ResponseDTO<bool>> ChangeAvatarAsync(int photoFileId);
        Task<ResponseDTO<bool>> DeleteAvatarAsync();
    }
}
