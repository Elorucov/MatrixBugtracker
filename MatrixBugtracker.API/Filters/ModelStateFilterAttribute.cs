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
                Dictionary<string, string> fields = new Dictionary<string, string>();
                foreach (var state in context.ModelState)
                {
                    fields.Add(state.Key, string.Join("; ", state.Value.Errors.Select(e => e.ErrorMessage)));
                }

                context.Result = new BadRequestObjectResult(ResponseDTO<object>.BadRequest("One of the parameters specified was invalid", fields));
            }
        }
    }
}
