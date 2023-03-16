using FluentAssertions;
using J4JSoftware.Logging;
using Microsoft.Extensions.Logging;

namespace MicrosoftTests;

public class InMemoryTests : TestBase
{
    [Theory]
    [InlineData("This is a test")]
    [InlineData("This is another test")]
    public void CachedTest( string mesg )
    {
        var cachedLogger = GetLogger(LogSinks.InMemory, LogLevel.Trace, string.Empty);
        cachedLogger.SourceCode().LogDebug(mesg);
        var inMemory = InMemorySink;
        inMemory.Should().NotBeNull();

        var logger = GetLogger(LogSinks.Debug | LogSinks.LastEvent, LogLevel.Trace, ContextTemplate);

        foreach( var logEvent in inMemory!.LogEvents )
        {
            logger.Log(logEvent.Level.ToMicrosoftLevel(), logEvent.MessageTemplate.Text, logEvent.Properties.ToPropertyValues());
        }

        LastEventSink.Should().NotBeNull();
        LastEventSink!.LastLogMessage.Should().NotBeNull();
        LastEventSink.LastLogMessage.Should().Be( $"[DBG] {mesg}\r\nCachedTest\r\nInMemoryTests.cs:15" );
    }
}