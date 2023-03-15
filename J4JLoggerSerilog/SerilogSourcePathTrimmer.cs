using System.Runtime.CompilerServices;
using Serilog.Core;
using Serilog.Events;

namespace J4JSoftware.Logging;

internal class SerilogSourcePathTrimmer : SourcePathTrimmer, ILogEventEnricher
{
    public SerilogSourcePathTrimmer(
        StringComparison? fileSystemComparer = null
    )
        : base(fileSystemComparer)
    {
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var srcPathProp = logEvent.Properties
            .FirstOrDefault(x => x.Key == LoggerTerms.CallerPathElementName)
            .Value?.ToString().Trim('"');

        if (string.IsNullOrEmpty(srcPathProp))
            return;

        // see if we can trim the path
        var matchingPath =
            SourceRootPaths.FirstOrDefault(x => srcPathProp.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0);

        if (string.IsNullOrEmpty(matchingPath))
            return;

        var revised = srcPathProp.Replace(matchingPath, string.Empty, FileSystemComparer);

        logEvent.AddOrUpdateProperty(UpdateSourcePath(propertyFactory, revised));
    }

    private LogEventProperty UpdateSourcePath(ILogEventPropertyFactory propertyFactory, string curPath)
    {
        return UpdateProperty(propertyFactory, curPath);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static LogEventProperty UpdateProperty(ILogEventPropertyFactory propertyFactory, string curPath)
    {
        return propertyFactory.CreateProperty(LoggerTerms.CallerPathElementName, curPath);
    }
}