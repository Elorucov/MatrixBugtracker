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

        [HttpGet]
        [AuthorizeApi]
        public async Task<IActionResult> GetAsync([FromQuery] GetReportsRequestDTO request)
        {
            return APIResponse(await _service.GetAsync(request));
        }

        [HttpGet("{reportId}")]
        [AuthorizeApi]
        public async Task<IActionResult> GetByIdAsync(int reportId)
        {
            return APIResponse(await _service.GetByIdAsync(reportId));
        }

        [HttpPost]
        [AuthorizeApi]
        public async Task<IActionResult> CreateAsync(ReportCreateDTO request)
        {
            return APIResponse(await _service.CreateAsync(request));
        }

        [HttpPatch]
        [AuthorizeApi]
        public async Task<IActionResult> EditAsync(ReportEditDTO request)
        {
            return APIResponse(await _service.EditAsync(request));
        }

        [HttpPatch("set-severity")]
        [AuthorizeApi([UserRole.Moderator, UserRole.Employee, UserRole.Admin])]
        public async Task<IActionResult> SetSeverityAsync(ReportPatchEnumDTO<ReportSeverity> request)
        {
            return APIResponse(await _service.SetSeverityAsync(request));
        }

        [HttpPatch("set-status")]
        [AuthorizeApi]
        public async Task<IActionResult> SetStatusAsync(ReportPatchEnumDTO<ReportStatus> request)
        {
            return APIResponse(await _service.SetStatusAsync(request));
        }

        [HttpPatch("set-reproduced")]
        [AuthorizeApi]
        public async Task<IActionResult> SetReproducedAsync([FromForm] int reportId)
        {
            return APIResponse(await _service.SetReproducedAsync(reportId, true));
        }

        [HttpPatch("unset-reproduced")]
        [AuthorizeApi]
        public async Task<IActionResult> UnsetReproducedAsync([FromForm] int reportId)
        {
            return APIResponse(await _service.SetReproducedAsync(reportId, false));
        }

        [HttpDelete("{reportId}")]
        [AuthorizeApi]
        public async Task<IActionResult> DeleteAsync(int reportId)
        {
            return APIResponse(await _service.DeleteAsync(reportId));
        }
    }
}
