#region license

// Copyright 2021 Mark A. Olbert
// 
// This library or program 'SerilogTests' is free software: you can redistribute it
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

namespace SerilogTests;

public class TestBase
{
    protected const string NoContextTemplate = "[{Level:u3}] {Message:lj}";

    protected const string ContextTemplate =
        "[{Level:u3}] {Message:lj}{NewLine}{CallerName}{NewLine}{CallerSourcePath}:{LineNumber}";

    private readonly TwilioConfiguration _twilioConfig;

    protected TestBase()
    {
        var configBuilder = new ConfigurationBuilder();

        var config = configBuilder
                    .AddUserSecrets<LoggingTests>()
                    .Build();

        var tempTwilio = new TwilioConfiguration();
        var section = config.GetSection( "Twilio" );
        section.Bind( tempTwilio );
        tempTwilio.Should().NotBeNull();
        tempTwilio.IsValid.Should().BeTrue();
        _twilioConfig = tempTwilio;
    }

    private void LogEvent( object? sender, NetEventArgs e ) => OnNetEvent( e );

    protected virtual void OnNetEvent( NetEventArgs e )
    {
    }

    protected ILogger GetLogger( LogSinks sinks, LogEventLevel minLevel, string outputTemplate )
    {
        LastEventSink = null;
        InMemorySink = null;

        var loggerConfig = minLevel switch
        {
            LogEventLevel.Verbose => new LoggerConfiguration().MinimumLevel.Verbose(),
            LogEventLevel.Debug => new LoggerConfiguration().MinimumLevel.Debug(),
            LogEventLevel.Information => new LoggerConfiguration().MinimumLevel.Information(),
            LogEventLevel.Warning => new LoggerConfiguration().MinimumLevel.Warning(),
            LogEventLevel.Error => new LoggerConfiguration().MinimumLevel.Error(),
            LogEventLevel.Fatal => new LoggerConfiguration().MinimumLevel.Fatal(),
            _ => throw new InvalidEnumArgumentException( $"Unsupported {typeof( LogEventLevel )} '{minLevel}'" )
        };

        loggerConfig = loggerConfig.Enrich.FromLogContext()
                                   .Enrich.WithSourcePathTrimmer( typeof( TestBase ) );

        if( ( sinks & LogSinks.Debug ) == LogSinks.Debug )
            loggerConfig = loggerConfig.WriteTo.Debug( minLevel, outputTemplate );

        if( ( sinks & LogSinks.InMemory ) == LogSinks.InMemory )
        {
            loggerConfig = loggerConfig.WriteTo.InMemory( out var temp );
            InMemorySink = temp;
        }

        if( ( sinks & LogSinks.LastEvent ) == LogSinks.LastEvent )
        {
            loggerConfig = loggerConfig.WriteTo.LastEvent( out var temp, minLevel, outputTemplate );
            LastEventSink = temp;
        }

        if( ( sinks & LogSinks.NetEvent ) == LogSinks.NetEvent )
            loggerConfig = loggerConfig.WriteTo.NetEvent( minLevel, outputTemplate );

        if( ( sinks & LogSinks.Twilio ) == LogSinks.Twilio )
            loggerConfig = loggerConfig.WriteTo.Twilio( _twilioConfig, minLevel, outputTemplate );

        return loggerConfig.CreateLogger();
    }

    protected string FormatTemplate( string message, LogEventLevel level, params object[] args )
    {
        var threeLetter = level switch
        {
            LogEventLevel.Verbose => "VRB",
            LogEventLevel.Information => "INF",
            LogEventLevel.Debug => "DBG",
            LogEventLevel.Warning => "WRN",
            LogEventLevel.Error => "ERR",
            LogEventLevel.Fatal => "FTL",
            _ => throw new InvalidEnumArgumentException( $"Unsupported {typeof( LogEventLevel )} value '{level}'" )
        };

        for( var idx = 0; idx < args.Length; idx++ )
        {
            var replacement = $"\"{args[ idx ]}\"";
            message = message.Replace( $"{{{idx}}}", replacement );
        }

        return $"[{threeLetter}] {message}";
    }

    protected LastEventSink? LastEventSink { get; private set; }
    protected InMemorySink? InMemorySink { get; private set; }
}