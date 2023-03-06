using FluentAssertions;
using J4JLoggingEnhancementTests;
using J4JSoftware.Logging;
using Serilog.Events;

namespace J4JLoggingEnhancementsTest;

public class CachedLoggingTests : TestBase
{
    [Fact]
    public void CachedTest()
    {
        var cachedLogger = new J4JCachedLogger();
        cachedLogger.Debug("This is a test");

        var logger = GetLogger(LogSinks.Debug | LogSinks.LastEvent, LogEventLevel.Verbose, ContextTemplate);

        cachedLogger.OutputToLogger(logger);

        LastEvent.Should().NotBeNull();
        LastEvent!.LastLogMessage.Should().NotBeNull();
        LastEvent.LastLogMessage.Should().Be("[DBG] This is a test\r\nCachedTest\r\nCachedLoggingTests.cs:14");
    }
}