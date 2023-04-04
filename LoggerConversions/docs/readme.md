# LoggerConversions

A library containing extension methods for converting between Microsoft and Serilog log levels. Part of the [J4JLoggerEnhancements](../../README.md) system.

Conforms to NET 7.

Nullability is enabled.

The libraries are licensed under the GNU GPL-v3 or later. For more details see the [license file](../../LICENSE.md).

See the [change log](changes.md) for a history of significant changes.

## Converters

The library contains two extension methods:

|Method Name|Purpose|
|-----------|-------|
|`ToMicrosoftLevel()`|converts a `Serilog` log event level to its corresponding `Microsoft` log level|
|`ToSerilogLevel()`|converts a `Microsoft` log level to its corresponding `Serilog` log event level|
