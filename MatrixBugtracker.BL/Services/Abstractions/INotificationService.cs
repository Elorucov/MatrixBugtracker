using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Notifications;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface INotificationService
    {
        Task<bool> SendToUserAsync(int targetUserId, bool sendEmail, UserNotificationKind kind, string text,
            LinkedEntityType? entityType = null, int? entityId = null);
        Task<bool> SendToUsersAsync(List<int> targetUserIds, bool sendEmail, UserNotificationKind kind, string text,
            LinkedEntityType? entityType = null, int? entityId = null);
        Task<ResponseDTO<int?>> SendToAllAsync(PlatformNotificationKind kind, string message);

        Task<PaginationResponseDTO<UserNotificationDTO>> GetUserNotificationsAsync(PaginationRequestDTO request);
    }
}
