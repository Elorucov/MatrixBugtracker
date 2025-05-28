using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly IReportsService _service;
        public ReportsController(IReportsService service)
        {
            _service = service;
        }

        [HttpGet]
        [AuthorizeApi]
        public IActionResult GetEnumValues()
        {
            return APIResponse(_service.GetEnumValues());
        }
    }
}
