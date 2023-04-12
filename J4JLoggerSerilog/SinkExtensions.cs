#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// SinkExtensions.cs
//
// This file is part of JumpForJoy Software's J4JLoggerSerilog.
// 
// J4JLoggerSerilog is free software: you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the 
// Free Software Foundation, either version 3 of the License, or 
// (at your option) any later version.
// 
// J4JLoggerSerilog is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
// 
// You should have received a copy of the GNU General Public License along 
// with J4JLoggerSerilog. If not, see <https://www.gnu.org/licenses/>.
#endregion

using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting;

namespace J4JSoftware.Logging;

public static class SinkExtensions
{
    public static LoggerConfiguration InMemory( this LoggerSinkConfiguration config, out InMemorySink sink )
    {
        sink = new InMemorySink();

        return config.Sink( sink, LogEventLevel.Verbose );
    }

    public static LoggerConfiguration LastEvent(
        this LoggerSinkConfiguration loggerConfig,
        out LastEventSink sink,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose,
        string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    {
        sink = new LastEventSink( outputTemplate );

        return loggerConfig.Sink( sink, restrictedToMinimumLevel );
    }

    public static LoggerConfiguration LastEvent(
        this LoggerSinkConfiguration loggerConfig,
        ITextFormatter textFormatter,
        out LastEventSink sink,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose
    )
    {
        sink = new LastEventSink( textFormatter );

        return loggerConfig.Sink( sink, restrictedToMinimumLevel );
    }

    public static LoggerConfiguration NetEvent(
        this LoggerSinkConfiguration loggerConfig,
        out NetEventSink sink,
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
        string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    {
        sink = new NetEventSink( outputTemplate );

        return loggerConfig.Sink( sink, restrictedToMinimumLevel );
    }

    public static LoggerConfiguration NetEvent(
        this LoggerSinkConfiguration loggerConfig,
        ITextFormatter textFormatter,
        out NetEventSink sink,
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
        string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    {
        sink = new NetEventSink( outputTemplate );

        return loggerConfig.Sink( new NetEventSink( textFormatter ), restrictedToMinimumLevel );
    }
}