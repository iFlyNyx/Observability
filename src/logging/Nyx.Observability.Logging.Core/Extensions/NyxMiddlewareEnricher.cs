using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Nyx.Observability.Logging.Core.Extensions;

public class NyxMiddlewareEnricher : IMiddleware
{
    private const string CorrelationHeader = "X-Correlation-ID";
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string? correlationId;
        
        try
        {
            correlationId = context.Request.Headers[CorrelationHeader].FirstOrDefault() ?? Guid.NewGuid().ToString();
            context.Request.Headers[CorrelationHeader] = correlationId;
        }
        catch (Exception)
        {
            correlationId = Guid.NewGuid().ToString();
        }

        using (LogContext.PushProperty("CorrelationId", correlationId));
        
        await next(context);
    }
}