using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/v1/product-membership")]
    public class ProductMembershipController : BaseController
    {
        private readonly IProductService _service;
        public ProductMembershipController(IProductService service)
        {
            _service = service;
        }

        [HttpPost("invite")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> InviteUser([FromForm] int productId, [FromForm] int userId)
        {
            return APIResponse(await _service.InviteUserAsync(productId, userId));
        }

        [HttpDelete("kick")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> KickUser([FromForm] int productId, [FromForm] int userId)
        {
            return APIResponse(await _service.KickUserAsync(productId, userId));
        }

        [HttpPost("join")]
        [AuthorizeApi([UserRole.Tester])]
        public async Task<IActionResult> Join([FromForm] int productId)
        {
            return APIResponse(await _service.JoinAsync(productId));
        }

        [HttpDelete("leave")]
        [AuthorizeApi([UserRole.Tester])]
        public async Task<IActionResult> Leave([FromForm] int productId)
        {
            return APIResponse(await _service.LeaveAsync(productId));
        }

        [HttpGet("invited-products")]
        [AuthorizeApi([UserRole.Tester])]
        public async Task<IActionResult> GetInvitedProducts([FromQuery] PaginationRequestDTO request)
        {
            return APIResponse(await _service.GetProductsWithInviteRequestAsync(request));
        }

        [HttpGet("join-requests")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> GetJoinRequests([FromQuery] GetJoinRequestUsersReqDTO request)
        {
            return APIResponse(await _service.GetJoinRequestUsers(request));
        }
    }
}
