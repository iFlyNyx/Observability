# Nyx Observability Platform

This solution is designed to be used as a baseline library for an observability platform. It provides a set of tools to be used in your development to enable standard and consistent features across your applications.

## Logging

Standardized logging is key to any observability platform. With configured standards you have an easier time parsing, analyzing, searching, and visualizing your logs to help identify and troubleshoot issues in your applications. The package provides a few different libraries to help you get started with logging and will be covered below.

For the purposes of this package, the logging platform is built around Serilog.

### Logging - AppSettings

The appsettings definitions for the core library is intended to be a custom object that is not directly dependent on the Serilog.Settings.Configuration package. This allows for a more flexible and customized approach for configuring the logging settings. However, the additional packages that are provided have logic interally to them to ensure that if the serilog appsettings is used, it will prefer those settings over the custom appsettings. This is to help avoid confusion and duplicate logging configuration from both packages.

A specific settings value is configureable to enable environment specific logging settings; IsEnabled. With this setting set to false, your application program/startup does not need to change as the underlying library will use this value to configure if the supplied sink should be enabled.

### Logging - Core

The core logging library provides the base functionality that other libraries are dependent upon. It has standard models for appsettings configurations, logging enrichers, and the base implementation of the logger itself. This library is not intended to be used directly and should be imported by the other libraries provided.

### Logging - Console

The console logging library provides a simple implementation of the Serilog.Sinks.Console package with standard templates and ansi coloring.

### Logging - File

The file logging library provides a simple implementation of the Serilog.Sinks.File package with standard templates, rolling file configuration, max file size, and retained file count. Utilization of this package expects the log path to be provided in the appsettings configuration or the sink will not be registered. There is potential to enable a fallback, but given complexity of file logging it was chosen to not be implemented at this time. 

The file logging library will internally utilize the Serilog.Sinks.Async package to enable buffering of events and offload the process to a background thread for performance reasons.

### Logging - EventLog

The event log logging library provides a simple implementation of the Serilog.Sinks.EventLog package with a standard template, default managed event source, and default name. As the EventLog is a windows specific sink, the libary will check the underlying operating system and only register the sink if the OS is windows. 

The EventLog logging library will internally utilize the Serilog.Sinks.Async package to enable buffering of events and offload the process to a background thread for performance reasons.

### Logging - Loki

The loki logging library provides a simple implementation of the Serilog.Sinks.Grafana.Loki package with default labels being pulled from the environment variables in addition to the provided values from appsettings. The library will not register the same label if it is provided in both places, but will prefer appsettings values over environment variables. Additionally, if no uri has been defined in the appsettings, the libary will not register the loki sink.

### Logging - Default

The default logging library is the 'batteries included' approach to the overall packages. It internally uses all sinks listed above and will attempt to register them based on the provided appsettings. This is not necessarily intended to be used in all cases as more direct control over the selected sinks may be desired, but it is provided as a quick and easy way to get up and running with some standard logging features.