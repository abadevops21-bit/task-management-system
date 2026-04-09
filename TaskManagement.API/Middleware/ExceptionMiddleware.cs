using System.Net;
using System.Text.Json;

namespace TaskManagementSystem.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.ContentType = "application/json";

                // Default
                var statusCode = (int)HttpStatusCode.InternalServerError;
                var message = "Something went wrong. Please try again later.";

                // Custom handling
                switch (ex)
                {
                    case UnauthorizedAccessException:
                        statusCode = StatusCodes.Status401Unauthorized;
                        message = "Unauthorized access";
                        break;

                    case KeyNotFoundException:
                        statusCode = StatusCodes.Status404NotFound;
                        message = ex.Message;
                        break;

                    case ArgumentException:
                        statusCode = StatusCodes.Status400BadRequest;
                        message = ex.Message;
                        break;
                }

                context.Response.StatusCode = statusCode;

                var response = new
                {
                    statusCode,
                    message = _env.IsDevelopment() ? ex.Message : message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}