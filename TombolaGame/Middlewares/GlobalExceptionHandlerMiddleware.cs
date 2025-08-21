using System.Text.Json;
using TombolaGame.Exceptions;

namespace TombolaGame.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                switch (ex)
                {
                    case InvalidOperationException _:
                    case ValidationException _:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        break;
                    case DuplicateEntryException _:
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        break;
                    case EntityNotFoundException _:
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        break;
                    default:
                        _logger.LogError(ex, "Unhandled exception");
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                var response = new { message = ex.Message };
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}