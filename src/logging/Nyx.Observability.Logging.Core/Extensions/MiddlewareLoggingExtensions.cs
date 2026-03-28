using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Nyx.Observability.Logging.Core.Extensions;

public static class MiddlewareLoggingExtensions
{
    public static IApplicationBuilder UseNyxMiddlewareLogging(this IApplicationBuilder app)
    {
        return app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress?.ToString());
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
            };
        });
    }
}