#region license

// Copyright 2021 Mark A. Olbert
// 
// This library or program 'J4JLoggingEnhancementTests' is free software: you can redistribute it
// and/or modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation, either version 3 of the License,
// or (at your option) any later version.
// 
// This library or program is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along with
// this library or program.  If not, see <https://www.gnu.org/licenses/>.

#endregion

using System.ComponentModel;
using FluentAssertions;
using J4JSoftware.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace J4JLoggingEnhancementTests;

[Flags]
public enum LogSinks
{
    Debug = 1 << 0,
    NetEvent = 1 << 1,
    LastEvent = 1 << 2,
    Twilio = 1 << 3,

    AllButTwilio = Debug | NetEvent | LastEvent,
    All = Debug | NetEvent | LastEvent | Twilio,
    None = 0
}

public class TestBase
{
    private readonly TwilioConfiguration _twilioConfig;

    protected TestBase()
    {
        var configBuilder = new ConfigurationBuilder();

        var config = configBuilder
            .AddUserSecrets<LoggingTests>()
            .Build();

        var tempTwilio = config.Get<TwilioConfiguration>();
        tempTwilio.Should().NotBeNull();
        _twilioConfig = tempTwilio!;
    }

    private void LogEvent( object? sender, NetEventArgs e ) => OnNetEvent( e );

    protected virtual void OnNetEvent( NetEventArgs e )
    {
    }

    protected ILogger GetLogger(LogSinks sinks, LogEventLevel minLevel)
    {
        LastEvent = null;

        var retVal = minLevel switch
        {
            LogEventLevel.Verbose => new LoggerConfiguration().MinimumLevel.Verbose(),
            LogEventLevel.Debug => new LoggerConfiguration().MinimumLevel.Debug(),
            LogEventLevel.Information => new LoggerConfiguration().MinimumLevel.Information(),
            LogEventLevel.Warning => new LoggerConfiguration().MinimumLevel.Warning(),
            LogEventLevel.Error => new LoggerConfiguration().MinimumLevel.Error(),
            LogEventLevel.Fatal => new LoggerConfiguration().MinimumLevel.Fatal(),
            _ => throw new InvalidEnumArgumentException($"Unsupported {typeof(LogEventLevel)} '{minLevel}'")
        };

        if ((sinks & LogSinks.Debug) == LogSinks.Debug)
            retVal = retVal.WriteTo.Debug(restrictedToMinimumLevel: minLevel);

        if ((sinks & LogSinks.LastEvent) == LogSinks.LastEvent)
        {
            retVal = retVal.WriteTo.LastEvent(out var temp, restrictedToMinimumLevel: minLevel);
            LastEvent = temp;
        }

        if ((sinks & LogSinks.NetEvent) == LogSinks.NetEvent)
            retVal = retVal.WriteTo.NetEvent(restrictedToMinimumLevel: minLevel);

        if ((sinks & LogSinks.Twilio) == LogSinks.Twilio)
            retVal = retVal.WriteTo.Twilio(_twilioConfig, restrictedToMinimumLevel: minLevel);

        return retVal.CreateLogger();
    }

    protected string FormatTemplate(string template, params object[] args)
    {
        for (var idx = 0; idx < args.Length; idx++)
        {
            var replacement = args[idx] is string stringVal ? $"\"{stringVal}\"" : args[idx].ToString();

            template = template.Replace($"{{{idx}}}", replacement);
        }

        return template;
    }

    protected LastEventSink? LastEvent { get; private set; }
}