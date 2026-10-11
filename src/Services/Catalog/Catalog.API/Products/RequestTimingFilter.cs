using System.Diagnostics;

namespace Catalog.API.Filters;

public sealed class RequestTimingFilter(ILogger<RequestTimingFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var startedAt = Stopwatch.GetTimestamp();

        var result = await next(context);

        var elapsed = Stopwatch.GetElapsedTime(startedAt);
        logger.LogInformation(
            "{Method} {Path} handled in {ElapsedMs:F1} ms",
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path,
            elapsed.TotalMilliseconds);

        return result;
    }
}