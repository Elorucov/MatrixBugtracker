using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Enums;
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

        [HttpGet("config")]
        [AuthorizeApi]
        public IActionResult GetEnumValues()
        {
            return APIResponse(_service.GetEnumValues());
        }

        [HttpPost]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> Create([FromForm] ProductCreateDTO request)
        {
            return APIResponse(await _service.CreateAsync(request));
        }

        [HttpPut]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> Edit([FromForm] ProductEditDTO request)
        {
            return APIResponse(await _service.EditAsync(request));
        }

        [HttpPatch("finish-testing")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> FinishTesting([FromForm] int productId)
        {
            return APIResponse(await _service.SetIsOverFlagAsync(productId, true));
        }

        [HttpPatch("resume-testing")]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> ResumeTesting([FromForm] int productId)
        {
            return APIResponse(await _service.SetIsOverFlagAsync(productId, false));
        }

        [HttpGet]
        [AuthorizeApi]
        public async Task<IActionResult> Get([FromQuery] PaginationRequestDTO request)
        {
            return APIResponse(await _service.GetAllAsync(request));
        }

        [HttpGet("search")]
        [AuthorizeApi]
        public async Task<IActionResult> Search([FromQuery] PaginatedSearchRequestDTO request)
        {
            return APIResponse(await _service.SearchAsync(request));
        }
    }
}
