# J4JLoggerMicrosoft

A library containing extension methods for including source code information in Microsoft log events. Part of the [J4JLoggerEnhancements](../../README.md) system.

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
