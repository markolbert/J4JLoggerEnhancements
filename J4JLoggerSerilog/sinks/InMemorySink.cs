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