using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using Infrastructure.Logging;

namespace Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILog _logger;

        public ExceptionMiddleware(RequestDelegate next, ILog logger)
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
            catch (Exception error)
            {
                _logger.Error($"The exception was thrown: " + error.Message + Environment.NewLine + error.StackTrace);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { message = error.Message });
                await context.Response.WriteAsync(result);
            }
        }
    }
}
