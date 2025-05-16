using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
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

        [HttpPut]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> FinishTesting([FromForm] int productId)
        {
            return APIResponse(await _service.SetIsOverFlagAsync(productId, true));
        }

        [HttpPut]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> ResumeTesting([FromForm] int productId)
        {
            return APIResponse(await _service.SetIsOverFlagAsync(productId, false));
        }

        [HttpPost]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> InviteUser([FromForm] int productId, [FromForm] int userId)
        {
            return APIResponse(await _service.InviteUserAsync(productId, userId));
        }

        [HttpPost]
        [AuthorizeApi([UserRole.Admin, UserRole.Employee])]
        public async Task<IActionResult> KickUser([FromForm] int productId, [FromForm] int userId)
        {
            return APIResponse(await _service.KickUserAsync(productId, userId));
        }

        [HttpPost]
        [AuthorizeApi([UserRole.Tester])]
        public async Task<IActionResult> Join([FromForm] int productId)
        {
            return APIResponse(await _service.JoinAsync(productId));
        }

        [HttpPost]
        [AuthorizeApi([UserRole.Tester])]
        public async Task<IActionResult> Leave([FromForm] int productId)
        {
            return APIResponse(await _service.LeaveAsync(productId));
        }
    }
}
