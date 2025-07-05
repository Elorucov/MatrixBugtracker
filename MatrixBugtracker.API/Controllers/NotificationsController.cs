using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Notifications;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    /// <summary>
    /// Methods for working with notifications
    /// </summary>
    [Route("api/v1/notifications")]
    [AuthorizeApi]
    public class NotificationsController : BaseController
    {
        private readonly INotificationService _service;
        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Send a notification for all users. Those notifications called as platform notifications
        /// </summary>
        /// <returns>New notification's ID</returns>
        [HttpPost("platform")]
        [AuthorizeApi([UserRole.Admin])]
        public async Task<IActionResult> SendPlatformNotificationAsync(SendPlatformNotificationRequestDTO request)
        {
            return APIResponse(await _service.SendToAllAsync(request));
        }

        /// <summary>
        /// Get notifications for an authenticated user
        /// </summary>
        /// <returns>List of notifications</returns>
        [HttpGet("my")]
        [AuthorizeApi]
        public async Task<IActionResult> GetUserNotificationsAsync([FromQuery] PaginationRequestDTO request)
        {
            return APIResponse(await _service.GetUserNotificationsAsync(request));
        }

        /// <summary>
        /// Get platform notifications
        /// </summary>
        /// <returns>List of platform notifications</returns>
        [HttpGet("platform")]
        [AuthorizeApi]
        public async Task<IActionResult> GetPlatformNotificationsAsync([FromQuery] PaginationRequestDTO request)
        {
            return APIResponse(await _service.GetPlatformNotificationsAsync(request));
        }

        /// <summary>
        /// Mark all unread notifications as read
        /// </summary>
        /// <returns>Count of unread notifications</returns>
        [HttpPatch("mark-all-as-read")]
        [AuthorizeApi]
        public async Task<IActionResult> MarkUserNotificationAsReadAsync()
        {
            return APIResponse(await _service.MarkAllUserNotificationsAsReadAsync());
        }

        /// <summary>
        /// Mark all unread (for authenticated user) platform notifications as read
        /// </summary>
        /// <returns>Count of unread notifications</returns>
        [HttpPatch("mark-all-platform-as-read")]
        [AuthorizeApi]
        public async Task<IActionResult> MarkPlatformNotificationAsReadAsync()
        {
            return APIResponse(await _service.MarkAllPlatformNotificationsAsReadAsync());
        }
    }
}
