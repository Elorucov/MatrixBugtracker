using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    /// <summary>
    /// Methods for manage products' members
    /// </summary>
    [Route("api/v1/product-membership")]
    public class ProductMembershipController : BaseController
    {
        private readonly IProductsService _service;
        public ProductMembershipController(IProductsService service)
        {
            _service = service;
        }

        /// <summary>
        /// Sends an invitation to a user to join the product
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPost("invite")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> InviteUserAsync(ProductUserRequestDTO request)
        {
            return APIResponse(await _service.InviteUserAsync(request));
        }

        /// <summary>
        /// Kick a user from product
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpDelete("kick")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> KickUserAsync(ProductUserRequestDTO request)
        {
            return APIResponse(await _service.KickUserAsync(request));
        }

        /// <summary>
        /// Joins an authenticated user to product (if open), or sends join request (if closed)
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPost("join")]
        [AuthorizeApi()]
        public async Task<IActionResult> JoinAsync([FromForm] int productId)
        {
            return APIResponse(await _service.JoinAsync(productId));
        }

        /// <summary>
        /// Exclude an authenticated user from product or revoke join request
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpDelete("leave")]
        [AuthorizeApi()]
        public async Task<IActionResult> LeaveAsync([FromForm] int productId)
        {
            return APIResponse(await _service.LeaveAsync(productId));
        }

        /// <summary>
        /// Gets a list of products that have an invite to
        /// </summary>
        /// <returns>List of products</returns>
        [HttpGet("invited")]
        [AuthorizeApi()]
        public async Task<IActionResult> GetInvitedProductsAsync([FromQuery] PaginationRequestDTO request)
        {
            return APIResponse(await _service.GetProductsWithInviteRequestAsync(request));
        }

        /// <summary>
        /// Gets a list of users that sent a join request to product
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet("join-requests")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> GetJoinRequestsAsync([FromQuery] GetMembersRequestDTO request)
        {
            return APIResponse(await _service.GetJoinRequestUsers(request));
        }
    }
}
