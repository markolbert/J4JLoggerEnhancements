using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace J4JSoftware.Logging;

// thanx to codea for the basic idea used here to pass context info!
// https://stackoverflow.com/questions/70851808/net-core-3-1-set-forcontext-for-serilog-with-microsoft-ilogger-interface
public static class LoggerExtensions
{
    public static ILogger SourceCode(
        this ILogger logger,
        bool include = true,
        [ CallerMemberName ] string callerName = "",
        [ CallerFilePath ] string sourceFilePath = "",
        [ CallerLineNumber ] int sourceLineNumber = 0
    )
    {
        if( !include )
            return logger;

        var forContext = new Dictionary<string, object>
        {
            { LoggerTerms.CallerMemberElementName, callerName },
            { LoggerTerms.CallerPathElementName, sourceFilePath },
            { LoggerTerms.CallerLineNumElementName, sourceLineNumber },
        };

        logger.BeginScope( forContext );
        return logger;
    }

    public static ILogger SendToSms( this ILogger logger, bool send = true )
    {
        if( !send )
            return logger;

        var forContext = new Dictionary<string, object> { { LoggerTerms.SendToSmsElementName, true } };

        using( logger.BeginScope( forContext ) )
        {
            return logger;
        }
    }
}