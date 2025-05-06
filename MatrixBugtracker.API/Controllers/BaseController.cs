using MatrixBugtracker.API.Filters;
using MatrixBugtracker.BL.DTOs.Infra;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ModelStateFilter]
    public class BaseController : ControllerBase
    {
        readonly int[] _statusCodesWithoutBody = {
            StatusCodes.Status204NoContent
        };

        public ObjectResult APIResponse<T>(ResponseDTO<T> response)
        {
            int code = response.HttpStatusCode;
            return StatusCode(code, !_statusCodesWithoutBody.Contains(code) ? response : null);
        }
    }
}
