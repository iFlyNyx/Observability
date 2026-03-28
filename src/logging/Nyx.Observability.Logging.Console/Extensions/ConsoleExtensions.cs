using Microsoft.Extensions.Configuration;
using Nyx.Observability.Logging.Core.Models;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Nyx.Observability.Logging.Console.Extensions;

public static class ConsoleExtensions
{
    private const string SerilogConsoleName = "Serilog.Sinks.Console";

    public static LoggerConfiguration OutputToConsole(this LoggerConfiguration logger, IConfiguration? configuration = null)
    {
        //need to check if serilog configuration is already defined for the console and if so, exit out before adding it a second time
        var serilogUsingList = configuration?.GetSection("Serilog:Using").Get<List<string>>();
        
        if (serilogUsingList?.Count > 0 && serilogUsingList.Contains(SerilogConsoleName, StringComparer.InvariantCultureIgnoreCase))
             return logger;
        
        var customSettings = configuration?.GetSection("NyxLoggingConfiguration:Console").Get<ConsoleSettings>() ?? new ConsoleSettings();

        if (!customSettings.IsEnabled)
            return logger;

        //var consoleTheme = AnsiConsoleTheme.Code;
        
        return logger
            .WriteTo.Console(
                outputTemplate: customSettings.OutputTemplate, 
                restrictedToMinimumLevel: customSettings.MinimumLevel,
                theme: AnsiConsoleTheme.Code);
    }
}