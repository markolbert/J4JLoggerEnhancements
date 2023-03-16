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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MicrosoftTests;

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

    protected ILogger GetLogger( LogSinks sinks, LogLevel minLevel, string outputTemplate )
    {
        LastEventSink = null;
        InMemorySink = null;

        var loggerConfig = minLevel switch
        {
            LogLevel.Trace => new LoggerConfiguration().MinimumLevel.Verbose(),
            LogLevel.Debug => new LoggerConfiguration().MinimumLevel.Debug(),
            LogLevel.Information => new LoggerConfiguration().MinimumLevel.Information(),
            LogLevel.Warning => new LoggerConfiguration().MinimumLevel.Warning(),
            LogLevel.Error => new LoggerConfiguration().MinimumLevel.Error(),
            LogLevel.Critical => new LoggerConfiguration().MinimumLevel.Fatal(),
            _ => throw new InvalidEnumArgumentException( $"Unsupported {typeof( LogLevel )} '{minLevel}'" )
        };

        loggerConfig = loggerConfig.Enrich.FromLogContext()
                                   .Enrich.WithSourcePathTrimmer( typeof( TestBase ) );

        var minSerilog = minLevel switch
        {
            LogLevel.Trace => LogEventLevel.Verbose,
            LogLevel.Debug => LogEventLevel.Debug,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Error => LogEventLevel.Error,
            LogLevel.Critical => LogEventLevel.Fatal,
            _ => throw new InvalidEnumArgumentException( $"Unsupported {typeof( LogLevel )} '{minLevel}'" )
        };

        if ( ( sinks & LogSinks.Debug ) == LogSinks.Debug )
            loggerConfig = loggerConfig.WriteTo.Debug( minSerilog, outputTemplate );

        if( ( sinks & LogSinks.InMemory ) == LogSinks.InMemory )
        {
            loggerConfig = loggerConfig.WriteTo.InMemory( out var temp );
            InMemorySink = temp;
        }

        if( ( sinks & LogSinks.LastEvent ) == LogSinks.LastEvent )
        {
            loggerConfig = loggerConfig.WriteTo.LastEvent( out var temp, minSerilog, outputTemplate );
            LastEventSink = temp;
        }

        if( ( sinks & LogSinks.NetEvent ) == LogSinks.NetEvent )
            loggerConfig = loggerConfig.WriteTo.NetEvent( minSerilog, outputTemplate );

        if( ( sinks & LogSinks.Twilio ) == LogSinks.Twilio )
            loggerConfig = loggerConfig.WriteTo.Twilio( _twilioConfig, minSerilog, outputTemplate );

        var msFactory = new LoggerFactory()
           .AddSerilog( loggerConfig.CreateLogger() );

        return msFactory.CreateLogger( this.GetType() );
    }

    protected string FormatTemplate( string message, LogLevel level, params object[] args )
    {
        var threeLetter = level switch
        {
            LogLevel.Trace => "VRB",
            LogLevel.Information => "INF",
            LogLevel.Debug => "DBG",
            LogLevel.Warning => "WRN",
            LogLevel.Error => "ERR",
            LogLevel.Critical => "FTL",
            _ => throw new InvalidEnumArgumentException( $"Unsupported {typeof( LogLevel )} value '{level}'" )
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