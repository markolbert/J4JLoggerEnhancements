using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace MicrosoftTests;

internal static class InMemoryExtensions
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

    public static List<object> ToPropertyValues( this IReadOnlyDictionary<string, LogEventPropertyValue> propertyValues )
    {
        return propertyValues.Select( x => x.Value ).Cast<object>().ToList();
    }
}
