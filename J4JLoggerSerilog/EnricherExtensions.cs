#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// 
// This file is part of J4JLogger.
//
// J4JLogger is free software: you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the 
// Free Software Foundation, either version 3 of the License, or 
// (at your option) any later version.
// 
// J4JLogger is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
// 
// You should have received a copy of the GNU General Public License along 
// with J4JLogger. If not, see <https://www.gnu.org/licenses/>.
#endregion

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