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
