using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.DTOs.Infra;
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
        public async Task<IActionResult> SendPlatformNotificationAsync([FromForm] PlatformNotificationKind kind, [FromForm]string message)
        {
            return APIResponse(await _service.SendToAllAsync(kind, message));
        }

        [HttpDelete("platform")]
        [AuthorizeApi([UserRole.Admin])]
        public async Task<IActionResult> DeletePlatformNotificationAsync([FromForm]int id)
        {
            await Task.Delay(1);
            return APIResponse(ResponseDTO<bool>.NotImplemented());
        }

        [HttpGet("user")]
        [AuthorizeApi]
        public async Task<IActionResult> GetUserNotificationsAsync()
        {
            await Task.Delay(1);
            return APIResponse(ResponseDTO<bool>.NotImplemented());
        }

        [HttpGet("platform")]
        [AuthorizeApi]
        public async Task<IActionResult> GetPlatformNotificationsAsync()
        {
            await Task.Delay(1);
            return APIResponse(ResponseDTO<bool>.NotImplemented());
        }

        [HttpPatch("mark-as-read")]
        [AuthorizeApi]
        public async Task<IActionResult> MarkUserNotificationAsReadAsync()
        {
            await Task.Delay(1);
            return APIResponse(ResponseDTO<bool>.NotImplemented());
        }

        [HttpPatch("mark-platform-as-read")]
        [AuthorizeApi]
        public async Task<IActionResult> MarkPlatformNotificationAsReadAsync()
        {
            await Task.Delay(1);
            return APIResponse(ResponseDTO<bool>.NotImplemented());
        }
    }
}
