using System.Diagnostics;
using TNTTBD.Services.Api.Common.Errors;
using TNTTBD.Services.Api.Common.Localization;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Api.Middleware;

/// <summary>
/// Global exception middleware
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext context,
        ILocalizationService localization,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            var response = new ErrorResponse
            {
                Status = StatusCodes.Status400BadRequest,
                ErrorCode = ex.ErrorCode,
                Message = localization.Get(ex.ErrorCode),
                TraceId = traceId
            };

            context.Response.StatusCode = 400;

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
            logger.LogError(ex, "Unhandled exception for {Method} {Path}. TraceId: {TraceId}",
                context.Request.Method, context.Request.Path, traceId);

            var response = new ErrorResponse
            {
                Status = 500,
                ErrorCode = "internal.server_error",
                Message = localization.Get("internal.server_error"),
                TraceId = traceId
            };

            context.Response.StatusCode = 500;

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}