using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/v1/products")]
    public class ProductsController : BaseController
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpPost]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> CreateAsync(ProductCreateDTO request)
        {
            return APIResponse(await _service.CreateAsync(request));
        }

        [HttpPut]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> EditAsync(ProductEditDTO request)
        {
            return APIResponse(await _service.EditAsync(request));
        }

        [HttpPatch("finish-testing")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> FinishTestingAsync([FromForm] int productId)
        {
            return APIResponse(await _service.ChangeIsOverFlagAsync(productId, true));
        }

        [HttpPatch("resume-testing")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> ResumeTestingAsync([FromForm] int productId)
        {
            return APIResponse(await _service.ChangeIsOverFlagAsync(productId, false));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetProductsRequestDTO request)
        {
            return APIResponse(await _service.GetAllAsync(request));
        }

        [HttpGet("joined")]
        public async Task<IActionResult> GetJoinedAsync([FromQuery] PaginationRequestDTO request)
        {
            return APIResponse(await _service.GetJoinedProductsAsync(request));
        }

        [HttpGet("{productId}")]
        [AuthorizeApi]
        public async Task<IActionResult> GetById(int productId)
        {
            return APIResponse(await _service.GetByIdAsync(productId));
        }
    }
}
