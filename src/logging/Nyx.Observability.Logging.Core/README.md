# Nyx Observability Logging Core

This library provides a simple wrapper around the [Serilog](https://serilog.net) logging library to allow for standardized information to your observability environment. It includes simple enrichers and property accessors to ensure that all logging sinks have access to the same information. 

At it's core, this library expects users to provide their configuration via appsettings.json files by utilizing the Serilog.Settings.Configuration package. This allows for a dynamic sink approach to logging, allowing users to easily add or remove their preferred sinks. Additionally, if you don't want to utilize the configuration approach, you can pass in a callback method to extend the logger configuration with your own custom configuration.

## Usage

This library has a few different extension methods to instantiate the logger. You can access the extension methods in your builder, depending on project type.

You are expected to import the sinks you want to utilize in your project. For example, if you want to utilize the Serilog.Sinks.Seq sink, you would need to import the appropriate nuget package. At that point you can either configure the sink in your appsettings.json file, or utilize the callback method to configure the sink in code.

If you'd like to read more on the appsettings.json configuration approach, you can read their documentation [here](https://github.com/serilog/serilog-settings-configuration)

## Standard .NET Applications

Example of usage in a standard application, such as a Worker Service:

```csharp
using Nyx.Observability.Logging.Core.Extensions;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) => 
    {
        services.UseNyxLogging(context.Configuration);
        //other service registrations
    })
    .Build();
```

If you'd like to extend the logger configuration with your own custom configuration, you can pass in a callback method:

```csharp
using Nyx.Observability.Logging.Core.Extensions;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) => 
    {
        services.UseNyxLogging(context.Configuration, options => 
        {
            options.Enrich.WithProperty("CustomProperty", "CustomValue");
            options.WriteTo.Console();
            //additional custom configuration
        });
        //other service registrations
    })
    .Build();
```

## Web Api Applications

Similarly, in a Web Api Application, you can utilize the same UseNyxLogging extension around your service registration. Additionally, you can utilize the middleware extensions provided to capture and append additional information to your logs.

```csharp
using Nyx.Observability.Logging.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.UseNyxLogging(builder.Configuration);

//optionally add custom configuration via callback
// builder.Services.UseNyxLogging(builder.Configuration, options =>
// {
//     options.Enrich.WithProperty("CustomProperty", "CustomValue");
//     options.WriteTo.Console();
//     //additional custom configuration
// });

var app = builder.Build();

app.UseNyxMiddlewareLogging();

//optionally add Nyx Middleware to append the correlation id to support tracing
app.UseMiddleware<NyxMiddlewareEnricher>();
//additional middleware registrations

app.Run();
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE.txt) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue if you have any suggestions or improvements.

## Acknowledgements

This library is built on top of the Serilog logging library and it's relevant Sinks, Enrichers, and Settings packages. Structured logging is a powerful tool for any observability strategy, and these packages provide a great foundation to build on.
