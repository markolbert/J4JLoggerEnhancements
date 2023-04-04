# J4JLoggerSerilog

A library containing extension methods for enhancing Serilog events and extending how they are routed. Part of the [J4JLoggerEnhancements](../../README.md) system.

Conforms to NET 7.

Nullability is enabled.

The libraries are licensed under the GNU GPL-v3 or later. For more details see the [license file](../../LICENSE.md).

See the [change log](changes.md) for a history of significant changes.

## Source Code Path Trimming

To include source code information, call the `SourceCode()` extension method on `ILogger`.

For more details refer to the [J4JLoggingCommon documentation](../../J4JLoggerCommon/docs/readme.md).

## SendToSmS

To send a log event to SMS, call the `SendToSms()` extension method on `ILogger`.

`SendToSms()` also accepts a boolean `send` parameter. When true, the default, the log event is sent to the previously-defined SMS endpoint. If set to false, the log event is not sent to SMS. This is to support only including some log events (i.e., by defining a variable that is true during debugging and false during production).

For information on setting up an SMS endpoint refer to the [J4JLoggerTwilio documentation](../../J4JLoggerTwilio/docs/readme.md).

## InMemorySink

A Serilog sink which stores log events in memory. I use it for caching log events while configuring an app's `IHost` and then emitting the cached events to the primary logger (which generally doesn't exist while the `IHost` is being initialized).

It can be used like this:

```csharp
var logger = loggerConfig
    .WriteTo
    .InMemory( out var temp )
    .CreateLogger();

// store for later use
var inMemorySink = temp;
```

## NetEventSink

A Serilog sink which raises NET events containing formatted log messages, which can be listened for and used in an app. I often use `NetEventSink` in Windows applications when I want to display log events in the UI.

It can be used like this:

```csharp
var logger = loggerConfig
    .WriteTo
    .NetEvent( out var temp )
    .CreateLogger();

temp.LogEvent += ( _, args ) => OnNetEvent( args );
```

## LastEventSink

A Serilog sink which holds the last log event in a pair of properties:

- `LastLogEvent`: the `LogEvent` itself
- `LastLogMessage`: the `LogEvent` rendered to text

While I can't recall ever having used `LastEventSink` in production, I find it quite useful from debugging logging problems.

It can be used like this:

```csharp
var logger = loggerConfig
    .WriteTo
    .LastEvent( out var temp )
    .CreateLogger();

// store the sink for future use
var lastEventSink = temp;
```
