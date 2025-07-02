using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Notifications;
using MatrixBugtracker.BL.Resources;
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
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationService> _logger;

        private readonly IUserNotificationRepository _userNotificationRepo;
        private readonly IPlatformNotificationRepository _platformNotificationRepo;

        public NotificationService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            IEmailService emailService, IUserIdProvider userIdProvider, IMapper mapper, ILogger<NotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _userIdProvider = userIdProvider;
            _mapper = mapper;
            _logger = logger;

            _userNotificationRepo = _unitOfWork.GetRepository<IUserNotificationRepository>();
            _platformNotificationRepo = _unitOfWork.GetRepository<IPlatformNotificationRepository>();
        }

        private string GetSubjectForNotificationKind(UserNotificationKind kind)
        {
            return kind switch
            {
                UserNotificationKind.ProductInvitation => Common.Email_ProductInvitation,
                UserNotificationKind.ProductJoinAccepted => Common.Email_ProductJoinAccepted,
                UserNotificationKind.ProductTestingFinished => Common.Email_ProductTestingFinished,
                UserNotificationKind.ReportCommentAdded => Common.Email_ReportCommentAdded,
                UserNotificationKind.RoleChanged => Common.Email_RoleChanged,
                UserNotificationKind.PasswordReset => Common.Email_PasswordReset,
                _ => Common.Email_DefaultSubject,
            };
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
                _ = Task.Factory.StartNew(async () =>
                {
                    string subject = GetSubjectForNotificationKind(kind);
                    await _emailService.SendMailAsync(target.Email, subject, string.Format(Common.EmailContent, target.FirstName, text));
                });
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
                // DI via class constructor leads to a crash on startup!
                var userService = _httpContextAccessor.HttpContext.RequestServices.GetService<IUserService>();
                var users = await userService.GetNamesAndEmailsAsync(targetUserIds);

                _ = Task.Factory.StartNew(async () =>
                {
                    string subject = GetSubjectForNotificationKind(kind);

                    foreach (var user in users)
                    {
                        await _emailService.SendMailAsync(user.Key, subject, string.Format(Common.EmailContent, user.Value, text));
                    }
                });
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
            int currentUserId = _userIdProvider.UserId;

            var result = await _userNotificationRepo.GetForUserAsync(currentUserId, request.Number, request.Size);

            List<UserNotificationDTO> notificationDTOs = _mapper.Map<List<UserNotificationDTO>>(result.Items);
            return new PaginationResponseDTO<UserNotificationDTO>(notificationDTOs, result.TotalCount);
        }

        public async Task<PaginationResponseDTO<PlatformNotificationDTO>> GetPlatformNotificationsAsync(PaginationRequestDTO request)
        {
            int currentUserId = _userIdProvider.UserId;

            var result = await _platformNotificationRepo.GetWithReadUsersAsync(request.Number, request.Size);

            List<PlatformNotificationDTO> notificationDTOs = new List<PlatformNotificationDTO>();
            foreach (var notification in result.Items)
            {
                PlatformNotificationDTO dto = _mapper.Map<PlatformNotificationDTO>(notification);
                dto.IsRead = notification.UsersThatRead.Any(n => n.UserId == currentUserId);
                notificationDTOs.Add(dto);
            }

            return new PaginationResponseDTO<PlatformNotificationDTO>(notificationDTOs, result.TotalCount);
        }

        public async Task<ResponseDTO<int?>> MarkAllUserNotificationsAsReadAsync()
        {
            int currentUserId = _userIdProvider.UserId;

            var notifications = await _userNotificationRepo.GetForUserUnreadAsync(currentUserId);
            if (notifications.Count == 0) return ResponseDTO<int?>.BadRequest(Errors.NotificationAlreadyRead);

            foreach (var notification in notifications)
            {
                notification.ViewedByTargetUser = true;
                _userNotificationRepo.Update(notification);
            }

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<int?>(notifications.Count);
        }

        public async Task<ResponseDTO<int?>> MarkAllPlatformNotificationsAsReadAsync()
        {
            int currentUserId = _userIdProvider.UserId;

            var readNotificationIds = await _platformNotificationRepo.GetReadNotificationIdsForUserAsync(currentUserId);
            var allNotificationIds = await _platformNotificationRepo.GetAllNotificationIdsAsync();
            var unreadNotificationIds = allNotificationIds.Except(readNotificationIds);

            foreach (var id in unreadNotificationIds)
            {
                await _platformNotificationRepo.MarkAsReadAsync(currentUserId, id);
            }

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<int?>(unreadNotificationIds.Count());
        }
    }
}
