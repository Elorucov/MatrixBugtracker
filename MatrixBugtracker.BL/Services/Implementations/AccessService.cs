using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Entities.Base;
using MatrixBugtracker.DAL.Enums;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class AccessService : IAccessService
    {
        private readonly IUserService _userService;
        private readonly IUserIdProvider _userIdProvider;

        public AccessService(IUserService userService, IUserIdProvider userIdProvider)
        {
            _userService = userService;
            _userIdProvider = userIdProvider;
        }

        public async Task<ResponseDTO<bool>> CheckAccessAsync(ICreateEntity entity)
        {
            int currentUserId = _userIdProvider.UserId;
            var user = await _userService.GetSingleUserAsync(currentUserId);

            if (user == null) return ResponseDTO<bool>.NotFound(Errors.NotFoundUser);
            if (user.Role != UserRole.Admin && entity.CreatorId != currentUserId)
                return ResponseDTO<bool>.Forbidden();

            return new ResponseDTO<bool>(true);
        }
    }
}
