using MatrixBugtracker.BL.DTOs.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MatrixBugtracker.API.Filters
{
    public class ModelStateFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var state in context.ModelState)
                {
                    foreach (var err in state.Value.Errors)
                    {
                        if (!string.IsNullOrEmpty(err.ErrorMessage))
                        {
                            errors.Add(err.ErrorMessage);
                        }
                    }
                }

                context.Result = new BadRequestObjectResult(ResponseDTO<object>.Error(StatusCodes.Status400BadRequest, string.Join("; ", errors)));
            }
        }
    }
}
