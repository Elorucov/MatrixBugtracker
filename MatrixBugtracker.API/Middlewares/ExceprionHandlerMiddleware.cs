using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.API.Middlewares
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                string reqPath = context.Request.Path;
                int httpStatusCode = StatusCodes.Status500InternalServerError;

                _logger.LogError($"An error occured while processing the request. Request path: {reqPath}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");

                context.Response.StatusCode = httpStatusCode;
                await context.Response.WriteAsJsonAsync(ResponseDTO<object>.Error(httpStatusCode, "Internal server error"));
            }
        }
    }
}
