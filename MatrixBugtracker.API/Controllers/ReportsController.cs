using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/v1/reports")]
    public class ReportsController : BaseController
    {
        private readonly IReportsService _service;
        public ReportsController(IReportsService service)
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
        [AuthorizeApi([UserRole.Tester])]
        public async Task<IActionResult> Create(ReportCreateDTO request)
        {
            return APIResponse(await _service.CreateAsync(request));
        }
    }
}
