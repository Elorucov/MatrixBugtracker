using AutoMapper;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationService> _logger;

        private readonly IUserNotificationRepository _userNotificationRepo;
        private readonly IPlatformNotificationRepository _platformNotificationRepo;

        public NotificationService(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper, ILogger<NotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;

            _userNotificationRepo = _unitOfWork.GetRepository<IUserNotificationRepository>();
            _platformNotificationRepo = _unitOfWork.GetRepository<IPlatformNotificationRepository>();
        }

        public async Task<bool> SendToUserAsync(int targetUserId, bool sendEmail, UserNotificationKind kind, LinkedEntityType? entityType = null, int? entityId = null)
        {
            User target = await _userService.GetSingleUserAsync(targetUserId);
            if (target == null)
            {
                _logger.LogError($"Cannot send notification to user {targetUserId} because requested user not found!");
                return false;
            }

            UserNotification notification = new UserNotification
            {
                Kind = kind,
                TargetUserId = targetUserId,
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

        public async Task<bool> GetUserNotificationsAsync(int userId)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

        public async Task<bool> GetPlatformNotificationsAsync(int userId)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }
    }
}
