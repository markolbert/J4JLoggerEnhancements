#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// ConversionExtensions.cs
//
// This file is part of JumpForJoy Software's LoggerConversions.
// 
// LoggerConversions is free software: you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the 
// Free Software Foundation, either version 3 of the License, or 
// (at your option) any later version.
// 
// LoggerConversions is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
// 
// You should have received a copy of the GNU General Public License along 
// with LoggerConversions. If not, see <https://www.gnu.org/licenses/>.
#endregion

using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace J4JSoftware.Logging;

public static class ConversionExtensions
{
    public static LogLevel ToMicrosoftLevel( this LogEventLevel serilogLevel ) =>
        serilogLevel switch
        {
            LogEventLevel.Verbose => LogLevel.Trace,
            LogEventLevel.Debug => LogLevel.Debug,
            LogEventLevel.Information => LogLevel.Information,
            LogEventLevel.Error => LogLevel.Error,
            LogEventLevel.Warning => LogLevel.Warning,
            LogEventLevel.Fatal => LogLevel.Critical,
            _ => throw new InvalidEnumArgumentException( $"Invalid {typeof( LogEventLevel )} '{serilogLevel}'" )
        };

    public static LogEventLevel ToSerilogLevel(this LogLevel msLevel) =>
        msLevel switch
        {
            LogLevel.Trace => LogEventLevel.Verbose,
            LogLevel.Debug => LogEventLevel.Debug,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Error => LogEventLevel.Error,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Critical=> LogEventLevel.Fatal,
            _ => throw new InvalidEnumArgumentException($"Invalid {typeof(LogLevel)} '{msLevel}'")
        };

    public static List<object> ToPropertyValues( this IReadOnlyDictionary<string, LogEventPropertyValue> propertyValues )
    {
        return propertyValues.Select( x => x.Value ).Cast<object>().ToList();
    }
}
