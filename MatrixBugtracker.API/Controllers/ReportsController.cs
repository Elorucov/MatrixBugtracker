using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    /// <summary>
    /// Methods for working with reports
    /// </summary>
    [Route("api/v1/reports")]
    public class ReportsController : BaseController
    {
        private readonly IReportsService _service;
        public ReportsController(IReportsService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns reports
        /// </summary>
        /// <remarks>
        /// An additional filter is applied to testers,
        /// which limits the receipt of other people's vulnerability reports
        /// and reports from non-open products in which they are not a member
        /// </remarks>
        /// <returns>List of reports with mentioned users and products</returns>
        [HttpGet]
        [AuthorizeApi]
        public async Task<IActionResult> GetAsync([FromQuery] GetReportsRequestDTO request)
        {
            return APIResponse(await _service.GetAsync(request));
        }

        /// <summary>
        /// Returns a report
        /// </summary>
        /// <remarks>
        /// Testers can not access to other people's vulnerability reports
        /// and reports from non-open products in which they are not a member.
        /// These applies to all other methods for working with reports.
        /// </remarks>
        /// <returns>Report</returns>
        [HttpGet("{reportId}")]
        [AuthorizeApi]
        public async Task<IActionResult> GetByIdAsync(int reportId)
        {
            return APIResponse(await _service.GetByIdAsync(reportId));
        }

        /// <summary>
        /// Create a report
        /// </summary>
        /// <remarks>
        /// It is not possible to create a report for a product whose testing has been finished
        /// </remarks>
        /// <returns>ID of created report</returns>
        [HttpPost]
        [AuthorizeApi]
        public async Task<IActionResult> CreateAsync(ReportCreateDTO request)
        {
            return APIResponse(await _service.CreateAsync(request));
        }

        /// <summary>
        /// Edit a report
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPatch]
        [AuthorizeApi]
        public async Task<IActionResult> EditAsync(ReportEditDTO request)
        {
            return APIResponse(await _service.EditAsync(request));
        }

        /// <summary>
        /// Change a severity of the report
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPatch("set-severity")]
        [AuthorizeApi([UserRole.Moderator, UserRole.Employee, UserRole.Admin])]
        public async Task<IActionResult> SetSeverityAsync(ReportPatchEnumDTO<ReportSeverity> request)
        {
            return APIResponse(await _service.SetSeverityAsync(request));
        }

        /// <summary>
        /// Change a status of the report
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPatch("set-status")]
        [AuthorizeApi]
        public async Task<IActionResult> SetStatusAsync(ReportPatchEnumDTO<ReportStatus> request)
        {
            return APIResponse(await _service.SetStatusAsync(request));
        }

        /// <summary>
        /// Mark report as reproduced by authenticated user
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPatch("set-reproduced")]
        [AuthorizeApi]
        public async Task<IActionResult> SetReproducedAsync([FromForm] int reportId)
        {
            return APIResponse(await _service.SetReproducedAsync(reportId, true));
        }

        /// <summary>
        /// Unmark report as reproduced by authenticated user
        /// </summary>
        /// <returns>'true' if success</returns>
        [HttpPatch("unset-reproduced")]
        [AuthorizeApi]
        public async Task<IActionResult> UnsetReproducedAsync([FromForm] int reportId)
        {
            return APIResponse(await _service.SetReproducedAsync(reportId, false));
        }

        /// <summary>
        /// Returns users who reproduced the issue in the report
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet("reproduced-users")]
        [AuthorizeApi]
        public async Task<IActionResult> GetReproducedUsersAsync(int reportId)
        {
            return APIResponse(await _service.GetReproducedUsersAsync(reportId));
        }

        /// <summary>
        /// Delete a report
        /// </summary>
        /// <remarks>
        /// It is not possible to delete the report that status is changed
        /// </remarks>
        /// <returns>'true' if success</returns>
        [HttpDelete("{reportId}")]
        [AuthorizeApi]
        public async Task<IActionResult> DeleteAsync(int reportId)
        {
            return APIResponse(await _service.DeleteAsync(reportId));
        }
    }
}
