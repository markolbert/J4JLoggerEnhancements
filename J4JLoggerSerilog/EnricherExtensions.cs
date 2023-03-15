using System.Reflection;
using Serilog;
using Serilog.Configuration;

namespace J4JSoftware.Logging;

public static class EnricherExtensions
{
    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        Assembly? assembly = null,
        StringComparison? fileSystemComparer = null
    )
    {
        assembly ??= Assembly.GetExecutingAssembly();

        var srcEnricher = new SerilogSourcePathTrimmer(fileSystemComparer);
        srcEnricher.AddAssembly(assembly);

        return enrichConfig.With(srcEnricher);
    }

    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        params Assembly[] assemblies
    ) => enrichConfig.WithSourcePathTrimmer(null, assemblies);

    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        StringComparison? fileSystemComparer = null,
        params Assembly[] assemblies
    )
    {
        var srcEnricher = new SerilogSourcePathTrimmer(fileSystemComparer);
        srcEnricher.AddAssemblies(assemblies);

        return enrichConfig.With(srcEnricher);
    }

    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        IEnumerable<Assembly> assemblies,
        StringComparison? fileSystemComparer = null
    )
    {
        var srcEnricher = new SerilogSourcePathTrimmer(fileSystemComparer);
        srcEnricher.AddAssemblies(assemblies);

        return enrichConfig.With(srcEnricher);
    }

    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        Type type,
        StringComparison? fileSystemComparer = null
    )
    {
        var srcEnricher = new SerilogSourcePathTrimmer(fileSystemComparer);
        srcEnricher.AddAssemblyFromType(type);

        return enrichConfig.With(srcEnricher);
    }

    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        params Type[] types) => enrichConfig.WithSourcePathTrimmer(null, types);

    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        StringComparison? fileSystemComparer = null,
        params Type[] types
    )
    {
        var srcEnricher = new SerilogSourcePathTrimmer(fileSystemComparer);
        srcEnricher.AddAssembliesFromTypes(types);

        return enrichConfig.With(srcEnricher);
    }

    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        IEnumerable<Type> types,
        StringComparison? fileSystemComparer = null
    )
    {
        var srcEnricher = new SerilogSourcePathTrimmer(fileSystemComparer);
        srcEnricher.AddAssembliesFromTypes(types);

        return enrichConfig.With(srcEnricher);
    }
}