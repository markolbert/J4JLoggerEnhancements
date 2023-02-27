using System.Runtime.CompilerServices;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace J4JSoftware.Logging;

public static class SinkExtensions
{
    public static LoggerConfiguration LastEvent(this LoggerSinkConfiguration loggerConfig,
        out LastEventSink sink,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose)
    {
        sink = new LastEventSink();

        return loggerConfig.Sink(sink, restrictedToMinimumLevel);
    }

    public static LoggerConfiguration NetEvent(this LoggerSinkConfiguration loggerConfig,
        string outputTemplate = NetEventSink.DefaultTemplate,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose) =>
        loggerConfig.Sink(new NetEventSink(outputTemplate), restrictedToMinimumLevel);

    public static ILogger SourceCode(this ILogger logger,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        return logger
            .ForContext("MemberName", memberName)
            .ForContext("FilePath", TemplateExtensions.TrimPath(sourceFilePath))
            .ForContext("LineNumber", sourceLineNumber);
    }

    public static ILogger SendToSms(this ILogger logger)
    {
        return logger.ForContext("SendToSms", true);
    }
}