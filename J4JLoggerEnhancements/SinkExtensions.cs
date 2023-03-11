using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting;

namespace J4JSoftware.Logging;

public static class SinkExtensions
{
    public static LoggerConfiguration InMemory(this LoggerSinkConfiguration config, out InMemorySink sink )
    {
        sink = new InMemorySink();

        return config.Sink(sink, LogEventLevel.Verbose);
    }

    public static LoggerConfiguration LastEvent(this LoggerSinkConfiguration loggerConfig,
        out LastEventSink sink,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose,
        string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    {
        sink = new LastEventSink(outputTemplate);

        return loggerConfig.Sink(sink, restrictedToMinimumLevel);
    }

    public static LoggerConfiguration LastEvent(this LoggerSinkConfiguration loggerConfig,
        ITextFormatter textFormatter,
        out LastEventSink sink,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose)
    {
        sink = new LastEventSink(textFormatter);

        return loggerConfig.Sink(sink, restrictedToMinimumLevel);
    }

    public static LoggerConfiguration NetEvent(this LoggerSinkConfiguration loggerConfig,
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
        string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}") =>
        loggerConfig.Sink(new NetEventSink(outputTemplate), restrictedToMinimumLevel);

    public static LoggerConfiguration NetEvent(this LoggerSinkConfiguration loggerConfig,
        ITextFormatter textFormatter,
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum) =>
        loggerConfig.Sink(new NetEventSink(textFormatter), restrictedToMinimumLevel);
}