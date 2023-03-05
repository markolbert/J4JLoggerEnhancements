using System.Reflection;
using Serilog;
using Serilog.Configuration;

namespace J4JSoftware.Logging;

public static class EnricherExtensions
{
    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        Assembly? assembly = null
    )
    {
        assembly ??= Assembly.GetExecutingAssembly();

        var srcEnricher = new SourcePathTrimmer();
        srcEnricher.AddAssembly(assembly);

        return enrichConfig.With(srcEnricher);
    }

    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        params Assembly[] assemblies
    )
    {
        var srcEnricher = new SourcePathTrimmer();
        srcEnricher.AddAssemblies(assemblies);

        return enrichConfig.With(srcEnricher);
    }

    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        IEnumerable<Assembly> assemblies
    )
    {
        var srcEnricher = new SourcePathTrimmer();
        srcEnricher.AddAssemblies(assemblies);

        return enrichConfig.With(srcEnricher);
    }

    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        Type type
    )
    {
        var srcEnricher = new SourcePathTrimmer();
        srcEnricher.AddAssemblyFromType(type);

        return enrichConfig.With(srcEnricher);
    }

    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        params Type[] types
    )
    {
        var srcEnricher = new SourcePathTrimmer();
        srcEnricher.AddAssembliesFromTypes(types);

        return enrichConfig.With(srcEnricher);
    }

    public static LoggerConfiguration WithSourcePathTrimmer(
        this LoggerEnrichmentConfiguration enrichConfig,
        IEnumerable<Type> types
    )
    {
        var srcEnricher = new SourcePathTrimmer();
        srcEnricher.AddAssembliesFromTypes(types);

        return enrichConfig.With(srcEnricher);
    }
}