using System.Reflection;
using System.Runtime.CompilerServices;
using Serilog;

namespace J4JSoftware.Logging;

public static class LoggerExtensions
{
    private static readonly Dictionary<string, string?> AssemblySourceRoots = new();

    public static ILogger SourceCode(
        this ILogger logger,
        bool include = true,
        [CallerMemberName] string callerName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!include)
            return logger;

        var srcRoot = GetSourceRoot(callerName);
        callerName = srcRoot == null ? callerName : callerName.Replace(srcRoot, string.Empty);

        return logger
            .ForContext(LoggerTerms.CallerMemberElementName, callerName)
            .ForContext(LoggerTerms.CallerPathElementName, sourceFilePath)
            .ForContext(LoggerTerms.CallerLineNumElementName, sourceLineNumber);
    }

    private static string? GetSourceRoot(string callerName)
    {
        if( AssemblySourceRoots.ContainsKey(callerName))
            return AssemblySourceRoots[callerName];

        var attr = Assembly.GetExecutingAssembly().GetCustomAttribute<SourceCodeRootPathAttribute>();
        AssemblySourceRoots.Add(callerName, attr?.RootPath);

        return attr?.RootPath;
    }

    public static ILogger SendToSms(this ILogger logger, bool send = true) =>
        send ? logger.ForContext(LoggerTerms.SendToSmsElementName, true) : logger;
}