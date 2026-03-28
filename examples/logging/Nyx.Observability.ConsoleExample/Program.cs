using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nyx.Observability.Logging.Console.Extensions;
using Nyx.Observability.Logging.Core.Extensions;

var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

var configuration = new ConfigurationBuilder()
    .AddJsonFile(path, optional: false, reloadOnChange: true)
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddConfiguration(configuration);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.UseNyxLogging(hostContext.Configuration, options =>
        {
            options.OutputToConsole(hostContext.Configuration);
        });
    })
    .Build();

using var serviceScopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var logger = serviceScopeFactory.ServiceProvider.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Hello World!");

await host.RunAsync();