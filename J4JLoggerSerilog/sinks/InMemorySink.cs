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

using System.Collections.ObjectModel;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace J4JSoftware.Logging;

public class InMemorySink : ILogEventSink
{
    private readonly List<LogEvent> _logEvents = new();

    public ReadOnlyCollection<LogEvent> LogEvents => _logEvents.AsReadOnly();

    public void Clear() => _logEvents.Clear();

    public void Emit(LogEvent logEvent)
    {
        _logEvents.Add(logEvent);
    }

    public void OutputTo(ILogger logger)
    {
        foreach (var logEvent in _logEvents)
        {
            logger.Write(logEvent);
        }
    }
}