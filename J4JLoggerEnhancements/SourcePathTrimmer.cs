using System.Reflection;
using System.Runtime.CompilerServices;
using Serilog.Core;
using Serilog.Events;

namespace J4JSoftware.Logging;

public class SourcePathTrimmer : ILogEventEnricher
{
    private readonly List<string> _srcRootPaths = new();
    private readonly StringComparison _fsComparer;

    public SourcePathTrimmer()
    {
        _fsComparer = Environment.OSVersion.Platform switch
        {
            PlatformID.MacOSX => StringComparison.Ordinal,
            PlatformID.Unix => StringComparison.Ordinal,
            PlatformID.Other => StringComparison.Ordinal,
            _ => StringComparison.OrdinalIgnoreCase
        };
    }

    public void AddAssembly(Assembly assembly) => AddSourceCodeRootPath(assembly);

    public void AddAssemblies(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            AddSourceCodeRootPath(assembly);
        }
    }

    public void AddAssemblies(IEnumerable<Assembly> assemblies) => AddAssemblies(assemblies.ToArray());

    private void AddSourceCodeRootPath(Assembly assembly)
    {
        if (assembly.GetCustomAttribute<SourceCodeRootPathAttribute>() is not { } attr)
            return;

        if (string.IsNullOrEmpty(attr.RootPath))
            return;

        // the wrapping call is needed to deal with / vs \ in Windows paths
        var path = Path.GetFullPath(attr.RootPath);
        if (!Path.EndsInDirectorySeparator(path))
            path = $"{path}{Path.DirectorySeparatorChar}";

        _srcRootPaths.Add(path);
    }

    public void AddAssemblyFromType(Type type) => AddSourceCodeRootPath(type.Assembly);

    public void AddAssembliesFromTypes(params Type[] types)
    {
        foreach (var type in types)
        {
            AddAssemblyFromType(type);
        }
    }

    public void AddAssembliesFromTypes(IEnumerable<Type> types) => AddAssembliesFromTypes(types.ToArray());

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var srcPathProp = logEvent.Properties
            .FirstOrDefault(x => x.Key == J4JLogger.CallerPathElementName)
            .Value?.ToString().Trim('"');

        if (string.IsNullOrEmpty(srcPathProp))
            return;

        // see if we can trim the path
        var matchingPath =
            _srcRootPaths.FirstOrDefault(x => srcPathProp.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0);

        if (string.IsNullOrEmpty(matchingPath))
            return;

        var revised = srcPathProp.Replace(matchingPath, string.Empty, _fsComparer);

        logEvent.AddOrUpdateProperty(UpdateSourcePath(propertyFactory,revised));
    }

    private LogEventProperty UpdateSourcePath(ILogEventPropertyFactory propertyFactory, string curPath )
    {
        return UpdateProperty(propertyFactory, curPath);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static LogEventProperty UpdateProperty(ILogEventPropertyFactory propertyFactory, string curPath)
    {
        return propertyFactory.CreateProperty(J4JLogger.CallerPathElementName, curPath);
    }
}