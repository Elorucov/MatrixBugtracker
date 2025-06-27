using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Entities.Base;
using MatrixBugtracker.Domain.Enums;

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

            if (user == null) return ResponseDTO<bool>.NotFound(Errors.NotFoundUser); // а нужен ли?
            if (user.Role != UserRole.Admin && entity.CreatorId != currentUserId)
                return ResponseDTO<bool>.Forbidden();

            return new ResponseDTO<bool>(true);
        }

        // Get entities that creator id is same with authorized user's id (if authorized user is tester),
        // otherwise return entities.
        public async Task<IEnumerable<T>> GetAccessibleEntitiesAsync<T>(IEnumerable<T> entities) where T : ICreateEntity
        {
            int currentUserId = _userIdProvider.UserId;
            var user = await _userService.GetSingleUserAsync(currentUserId);

            if (user.Role == UserRole.Admin) return entities;

            var owned = entities.Where(e => e.CreatorId == currentUserId);
            return owned;
        }
    }
}
