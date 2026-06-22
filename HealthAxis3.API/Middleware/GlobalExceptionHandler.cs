using HealthAxis3.API.Exceptions;
using HealthAxis3.Shared.Models.Dtos;
using Microsoft.AspNetCore.Diagnostics;

namespace HealthAxis3.API.Middleware
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An Unexpected error occured : {Message}", exception.Message);
            var (Statuscode, Message) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                InvalidException => (StatusCodes.Status400BadRequest, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, exception.Message) //General Exception Handler
            };
            var response = new ErrorResponse
            {
                StatusCode = Statuscode,
                Message = Message,
                Timestamp = DateTime.UtcNow,
                Path = httpContext.Request.Path
            };
            httpContext.Response.StatusCode = Statuscode;
            await httpContext.Response.WriteAsJsonAsync(response, httpContext.RequestAborted);
            return true;
        }
    }
}
