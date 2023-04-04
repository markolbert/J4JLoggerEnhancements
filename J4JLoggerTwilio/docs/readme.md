# J4JLoggerTwilio

This library provides a [Serilog](https://serilog.net/) sink which can be used to send SMS messages via [Twilio](https://www.twilio.com/).

Licensed under GNU GPL-v3.0. See the [license file](../../license.md) for details.

See the [change log](changes.md) for a history of changes to the library.

## Adding a TwilioSink

Adding a `TwilioSink` to `Serilog` involves defining an instance of `TwilioConfiguration` and then creating the sink from it.

This example assumes the required configuration information is contained in a user-secrets cache since I don't want my Twilio credentials available publicly. It also does not contain any recipient phone numbers so it will not work as shown:

```csharp
var configBuilder = new ConfigurationBuilder();

var config = configBuilder
            .AddUserSecrets<LoggingTests>()
            .Build();

var twilioConfig = new TwilioConfiguration();

// this next line assumes the Twilio parameters (e.g., account SID)
// are contained in 'JSON section' labled 'Twilio'
var section = config.GetSection( "Twilio" );
section.Bind( twilioConfig );

var logger = loggerConfig
    .WriteTo
    .Twilio( twilioConfig )
    .CreateLogger();
```
