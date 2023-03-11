using FluentAssertions;
using J4JLoggingEnhancementTests;
using J4JSoftware.Logging;
using Serilog.Events;

namespace J4JLoggingEnhancementsTest;

public class InMemoryTests : TestBase
{
    [Fact]
    public void CachedTest()
    {
        var cachedLogger = GetLogger(LogSinks.InMemory, LogEventLevel.Verbose, string.Empty);
        cachedLogger.SourceCode().Debug("This is a test");
        var inMemory = InMemorySink;
        inMemory.Should().NotBeNull();

        var logger = GetLogger(LogSinks.Debug | LogSinks.LastEvent, LogEventLevel.Verbose, ContextTemplate);

        inMemory!.OutputTo(logger);

        LastEventSink.Should().NotBeNull();
        LastEventSink!.LastLogMessage.Should().NotBeNull();
        LastEventSink.LastLogMessage.Should().Be("[DBG] This is a test\r\nCachedTest\r\nInMemoryTests.cs:14");
    }
}