using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    /// <summary>
    /// Methods for working with products
    /// </summary>
    [Route("api/v1/products")]
    public class ProductsController : BaseController
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <returns>ID of created product</returns>
        [HttpPost]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> CreateAsync(ProductCreateDTO request)
        {
            return APIResponse(await _service.CreateAsync(request));
        }

        /// <summary>
        /// Edit a product
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPut]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> EditAsync(ProductEditDTO request)
        {
            return APIResponse(await _service.EditAsync(request));
        }

        /// <summary>
        /// Mark product testing as finished
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPatch("finish-testing")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> FinishTestingAsync([FromForm] int productId)
        {
            return APIResponse(await _service.ChangeIsOverFlagAsync(productId, true));
        }

        /// <summary>
        /// Unmark product testing as finished.
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPatch("resume-testing")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> ResumeTestingAsync([FromForm] int productId)
        {
            return APIResponse(await _service.ChangeIsOverFlagAsync(productId, false));
        }

        /// <summary>
        /// Get products 
        /// </summary>
        /// <remarks>
        /// Testers can get only all open and closed products, and secret products that he joined to
        /// </remarks>
        /// <returns>List of products</returns>
        [HttpGet]
        [AuthorizeApi]
        public async Task<IActionResult> GetAsync([FromQuery] GetProductsRequestDTO request)
        {
            return APIResponse(await _service.GetAllAsync(request));
        }

        /// <summary>
        /// Gets products that the user is a member of
        /// </summary>
        /// <returns>List of products</returns>
        [HttpGet("joined")]
        [AuthorizeApi]
        public async Task<IActionResult> GetJoinedAsync([FromQuery] PaginationRequestDTO request)
        {
            return APIResponse(await _service.GetJoinedProductsAsync(request));
        }

        /// <summary>
        /// Gets members of a product
        /// </summary>
        /// <returns>List of products</returns>
        [HttpGet("members")]
        [AuthorizeApi]
        public async Task<IActionResult> GetProductMembersAsync([FromQuery] GetMembersRequestDTO request)
        {
            return APIResponse(await _service.GetMembersAsync(request));
        }

        /// <summary>
        /// Returns an info about the product
        /// </summary>
        /// <returns>Product with extended info</returns>
        [HttpGet("{productId}")]
        [AuthorizeApi]
        public async Task<IActionResult> GetById(int productId)
        {
            return APIResponse(await _service.GetByIdAsync(productId));
        }
    }
}
