# J4JLoggerEnhancements

Enhancements to Serilog ILogger to provide source code information and enable SMS logging.

## Overview

There are five assemblies defining the enhancement system:

|Assembly|Description|
|--------|-----------|
|[J4JLoggerCommon](J4JLoggerCommon/docs/readme.md)|common constants and support for trimming long source code paths|
|[J4JLoggerMicrosoft](J4JLoggerMicrosoft/docs/readme.md)|extensions for incorporating source code information into Microsoft logger events|
|[J4JLoggerSerilog](J4JLoggerSerilog/docs/readme.md)|extensions for enhancing Serilog events and extending how they are routed|
|[J4JLoggerTwilio](J4JLoggerTwilio/docs/readme.md)|enable log events to be sent as SMS messages (currently only supports Serilo|
|[LoggerConversions](LoggerConversions/docs/readme.md)|extensions to convert between Serilog and Microsoft log event levels|
