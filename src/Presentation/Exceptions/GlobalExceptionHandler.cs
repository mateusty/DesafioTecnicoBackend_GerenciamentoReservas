using System.Security.Principal;

using Application.Booking.Exceptions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Exceptions

{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
            )
        {
            _logger.LogError(exception, "An unhandled exception occurred, {Message}", exception.Message);

            var (statusCode, title) = MapExceptions(exception);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);


            return true;
        }

        public static (int StatusCode, string title) MapExceptions(Exception ex)
        {
            return ex switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                MissingBodyException => (StatusCodes.Status400BadRequest, "Bad Request"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };
        }
    }
}
