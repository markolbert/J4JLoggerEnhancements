using System.Reflection;
using System.Runtime.CompilerServices;
using Serilog;

namespace J4JSoftware.Logging;

public static class J4JLogger
{
    public const string CallerMemberElementName = "CallerName";
    public const string CallerPathElementName = "CallerSourcePath";
    public const string CallerLineNumElementName = "LineNumber";
    public const string SendToSmsElementName = "SendToSms";

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
            .ForContext(CallerMemberElementName, callerName)
            .ForContext(CallerPathElementName, sourceFilePath)
            .ForContext(CallerLineNumElementName, sourceLineNumber);
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
        send ? logger.ForContext(SendToSmsElementName, true) : logger;
}