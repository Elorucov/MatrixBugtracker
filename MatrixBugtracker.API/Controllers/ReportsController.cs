using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
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

        [HttpGet]
        [AuthorizeApi]
        public async Task<IActionResult> GetAsync([FromQuery] GetReportsRequestDTO request)
        {
            return APIResponse(await _service.GetAsync(request));
        }

        [HttpGet("{id}")]
        [AuthorizeApi]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return APIResponse(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        [AuthorizeApi([UserRole.Tester])]
        public async Task<IActionResult> CreateAsync([FromBody] ReportCreateDTO request)
        {
            return APIResponse(await _service.CreateAsync(request));
        }

        [HttpPatch]
        [AuthorizeApi([UserRole.Tester])]
        public async Task<IActionResult> EditAsync([FromBody] ReportEditDTO request)
        {
            return APIResponse(await _service.EditAsync(request));
        }

        [HttpPatch("set-severity")]
        [AuthorizeApi([UserRole.Moderator, UserRole.Employee, UserRole.Admin])]
        public async Task<IActionResult> SetSeverityAsync([FromForm] ReportPatchEnumDTO<ReportSeverity> request)
        {
            return APIResponse(await _service.SetSeverityAsync(request));
        }

        [HttpPatch("set-status")]
        [AuthorizeApi]
        public async Task<IActionResult> SetStatusAsync([FromForm] ReportPatchEnumDTO<ReportStatus> request)
        {
            return APIResponse(await _service.SetStatusAsync(request));
        }

        [HttpPatch("set-reproduced")]
        [AuthorizeApi]
        public async Task<IActionResult> SetReproducedAsync([FromForm] int reportId, [FromForm] bool reproduced)
        {
            return APIResponse(await _service.SetReproducedAsync(reportId, reproduced));
        }

        [HttpDelete]
        [AuthorizeApi]
        public async Task<IActionResult> DeleteAsync(int reportId)
        {
            return APIResponse(await _service.DeleteAsync(reportId));
        }
    }
}
