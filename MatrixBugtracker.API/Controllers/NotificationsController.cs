using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Notifications;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/v1/notifications")]
    [AuthorizeApi]
    public class NotificationsController : BaseController
    {
        private readonly INotificationService _service;
        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        [HttpPost("platform")]
        [AuthorizeApi([UserRole.Admin])]
        public async Task<IActionResult> SendPlatformNotificationAsync(SendPlatformNotificationRequestDTO request)
        {
            return APIResponse(await _service.SendToAllAsync(request));
        }

        [HttpGet("user")]
        [AuthorizeApi]
        public async Task<IActionResult> GetUserNotificationsAsync([FromQuery] PaginationRequestDTO request)
        {
            return APIResponse(await _service.GetUserNotificationsAsync(request));
        }

        [HttpGet("platform")]
        [AuthorizeApi]
        public async Task<IActionResult> GetPlatformNotificationsAsync([FromQuery] PaginationRequestDTO request)
        {
            return APIResponse(await _service.GetPlatformNotificationsAsync(request));
        }

        [HttpPatch("mark-all-user-as-read")]
        [AuthorizeApi]
        public async Task<IActionResult> MarkUserNotificationAsReadAsync()
        {
            return APIResponse(await _service.MarkAllUserNotificationsAsReadAsync());
        }

        [HttpPatch("mark-all-platform-as-read")]
        [AuthorizeApi]
        public async Task<IActionResult> MarkPlatformNotificationAsReadAsync()
        {
            return APIResponse(await _service.MarkAllPlatformNotificationsAsReadAsync());
        }
    }
}
