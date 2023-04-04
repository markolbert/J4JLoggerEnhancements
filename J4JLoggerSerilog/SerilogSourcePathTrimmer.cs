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