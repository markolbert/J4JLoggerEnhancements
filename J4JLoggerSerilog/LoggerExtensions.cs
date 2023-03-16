using System.Runtime.CompilerServices;
using Serilog;

namespace J4JSoftware.Logging;

public static class LoggerExtensions
{
    public static ILogger SourceCode(
        this ILogger logger,
        bool include = true,
        [CallerMemberName] string callerName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!include)
            return logger;

        return logger
            .ForContext(LoggerTerms.CallerMemberElementName, callerName)
            .ForContext(LoggerTerms.CallerPathElementName, sourceFilePath)
            .ForContext(LoggerTerms.CallerLineNumElementName, sourceLineNumber);
    }

    public static ILogger SendToSms(this ILogger logger, bool send = true) =>
        send ? logger.ForContext(LoggerTerms.SendToSmsElementName, true) : logger;
}