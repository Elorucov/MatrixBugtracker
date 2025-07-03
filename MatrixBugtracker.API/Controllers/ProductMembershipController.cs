using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
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
        public async Task<IActionResult> InviteUserAsync(ProductUserRequestDTO request)
        {
            return APIResponse(await _service.InviteUserAsync(request));
        }

        [HttpDelete("kick")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> KickUserAsync(ProductUserRequestDTO request)
        {
            return APIResponse(await _service.KickUserAsync(request));
        }

        [HttpPost("join")]
        [AuthorizeApi([UserRole.Tester])]
        public async Task<IActionResult> JoinAsync([FromForm] int productId)
        {
            return APIResponse(await _service.JoinAsync(productId));
        }

        [HttpDelete("leave")]
        [AuthorizeApi([UserRole.Tester])]
        public async Task<IActionResult> LeaveAsync([FromForm] int productId)
        {
            return APIResponse(await _service.LeaveAsync(productId));
        }

        [HttpGet("get-invited")]
        [AuthorizeApi([UserRole.Tester])]
        public async Task<IActionResult> GetInvitedProductsAsync([FromQuery] PaginationRequestDTO request)
        {
            return APIResponse(await _service.GetProductsWithInviteRequestAsync(request));
        }

        [HttpGet("get-join-requests")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> GetJoinRequestsAsync([FromQuery] GetJoinRequestUsersReqDTO request)
        {
            return APIResponse(await _service.GetJoinRequestUsers(request));
        }
    }
}
