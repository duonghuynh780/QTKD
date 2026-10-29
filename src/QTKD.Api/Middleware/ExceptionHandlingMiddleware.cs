using Microsoft.AspNetCore.Mvc;
using QTKD.Api.Exceptions;

namespace QTKD.Api.Middleware;

/// <summary>
/// Xử lý exception thống nhất: AppException trả đúng status code kèm message;
/// lỗi bất ngờ được ghi log và trả 500 chung chung, không lộ chi tiết nội bộ.
/// </summary>
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
        catch (AppException ex)
        {
            await WriteProblemAsync(context, ex.StatusCode, ex.Title, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception on {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError,
                "Internal Server Error", "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, int statusCode, string title, string detail)
    {
        if (context.Response.HasStarted)
            return;

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
        });
    }
}
