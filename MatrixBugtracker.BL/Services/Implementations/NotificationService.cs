using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Notifications;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationService> _logger;

        private readonly IUserNotificationRepository _userNotificationRepo;
        private readonly IPlatformNotificationRepository _platformNotificationRepo;

        public NotificationService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            IUserIdProvider userIdProvider, IMapper mapper, ILogger<NotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userIdProvider = userIdProvider;
            _mapper = mapper;
            _logger = logger;

            _userNotificationRepo = _unitOfWork.GetRepository<IUserNotificationRepository>();
            _platformNotificationRepo = _unitOfWork.GetRepository<IPlatformNotificationRepository>();
        }

        public async Task<bool> SendToUserAsync(int targetUserId, bool sendEmail, UserNotificationKind kind, string text, 
            LinkedEntityType? entityType = null, int? entityId = null)
        {
            // DI via class constructor leads to a crash on startup!
            var userService = _httpContextAccessor.HttpContext.RequestServices.GetService<IUserService>();
            User target = await userService.GetSingleUserAsync(targetUserId);
            if (target == null)
            {
                _logger.LogError($"Cannot send notification to user {targetUserId} because requested user not found!");
                return false;
            }

            UserNotification notification = new UserNotification
            {
                Kind = kind,
                TargetUserId = targetUserId,
                Text = text,
                LinkedEntityType = entityType,
                LinkedEntityId = entityId
            };

            await _userNotificationRepo.AddAsync(notification);

            if (sendEmail)
            {
                // TODO: email
            }

            return true;
        }

        public async Task<bool> SendToUsersAsync(List<int> targetUserIds, bool sendEmail, UserNotificationKind kind, string text,
            LinkedEntityType? entityType = null, int? entityId = null)
        {
            foreach (int userId in targetUserIds)
            {
                UserNotification notification = new UserNotification
                {
                    Kind = kind,
                    TargetUserId = userId,
                    Text = text,
                    LinkedEntityType = entityType,
                    LinkedEntityId = entityId
                };

                await _userNotificationRepo.AddAsync(notification);
            }

            if (sendEmail)
            {
                // TODO: email (as parallel/concurrent task)
            }

            return true;
        }

        // Note: we don't send email to all users!
        public async Task<ResponseDTO<int?>> SendToAllAsync(PlatformNotificationKind kind, string message)
        {
            PlatformNotification notification = new PlatformNotification
            {
                Kind = kind,
                Text = message
            };

            await _platformNotificationRepo.AddAsync(notification);
            await _unitOfWork.CommitAsync();

            return new ResponseDTO<int?>(notification.Id);
        }

        public async Task<PaginationResponseDTO<UserNotificationDTO>> GetUserNotificationsAsync(PaginationRequestDTO request)
        {
            // DI via class constructor leads to a crash on startup!
            var userService = _httpContextAccessor.HttpContext.RequestServices.GetService<IUserService>();

            int currentUserId = _userIdProvider.UserId;
            User currentUser = await userService.GetSingleUserAsync(currentUserId);

            var result = await _userNotificationRepo.GetForUserAsync(currentUserId, request.Number, request.Size);

            List<UserNotificationDTO> notificationDTOs = _mapper.Map<List<UserNotificationDTO>>(result.Items);
            return new PaginationResponseDTO<UserNotificationDTO>(notificationDTOs, result.TotalCount);
        }

        public async Task<bool> GetPlatformNotificationsAsync(int userId)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }
    }
}
