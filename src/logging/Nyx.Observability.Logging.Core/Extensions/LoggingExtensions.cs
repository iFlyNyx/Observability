using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;

namespace Nyx.Observability.Logging.Core.Extensions;

public static class LoggingExtensions
{
    public static IServiceCollection UseNyxLogging(this IServiceCollection services, IConfiguration configuration, Action<LoggerConfiguration>? configure = null)
    {
        return services.AddSerilog(config =>
        {
            config
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("ApplicationName", AppDomain.CurrentDomain.FriendlyName);
            
            //enable custom extensions from the user
            configure?.Invoke(config);
        });
    }
}