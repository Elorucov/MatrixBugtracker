using MatrixBugtracker.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MatrixBugtracker.API.Controllers
{
    [Route("api/v1/comments")]
    [AuthorizeApi]
    public class NotificationsController : BaseController
    {
    }
}
