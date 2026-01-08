using FluentValidation;
using System.Net;
using System.Text.Json;

namespace CoupleCentsAPI.Common.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            ValidationException validationEx => new
            {
                error = "Validation failed",
                details = validationEx.Errors.Select(e => e.ErrorMessage).ToList()
            },
            ArgumentException => new
            {
                error = "Invalid argument",
                details = new List<string> { exception.Message }
            },
            KeyNotFoundException => new
            {
                error = "Resource not found",
                details = new List<string> { exception.Message }
            },
            UnauthorizedAccessException => new
            {
                error = "Unauthorized",
                details = new List<string> { "Access denied" }
            },
            _ => new
            {
                error = "An error occurred",
                details = new List<string> { "Internal server error" }
            }
        };

        context.Response.StatusCode = exception switch
        {
            ValidationException => (int)HttpStatusCode.BadRequest,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}