﻿#region license

// Copyright 2021 Mark A. Olbert
// 
// This library or program 'MicrosoftTests' is free software: you can redistribute it
// and/or modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation, either version 3 of the License,
// or (at your option) any later version.
// 
// This library or program is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along with
// this library or program.  If not, see <https://www.gnu.org/licenses/>.

#endregion

using FluentAssertions;
using J4JSoftware.Logging;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace MicrosoftTests;

public class LoggingTests : TestBase
{
    [Theory]
    [InlineData(LogSinks.Debug | LogSinks.LastEvent, LogLevel.Trace)]
    [InlineData(LogSinks.Debug | LogSinks.Twilio, LogLevel.Trace)]
    public void WithoutSourceCodeInfo(LogSinks sinks, LogLevel level)
    {
        var message = "This is a {0} log event to {1}";

        var logger = GetLogger(sinks, LogLevel.Trace, NoContextTemplate);
        logger.Log( level, message, level, sinks );

        if ((sinks & LogSinks.LastEvent) != LogSinks.LastEvent)
            return;

        LastEventSink.Should().NotBeNull();
        LastEventSink!.LastLogMessage.Should().NotBeNull();

        var result = FormatTemplate(message, level, level, sinks.ToString());
        LastEventSink.LastLogMessage!.Should().Be(result);
    }

    [Theory]
    [InlineData(LogSinks.Debug | LogSinks.LastEvent, LogLevel.Trace, "WithSourceCodeInfo", "LoggingTests.cs", 57 )]
    [InlineData(LogSinks.Debug | LogSinks.Twilio, LogLevel.Trace, "WithSourceCodeInfo", "LoggingTests.cs", 57)]
    public void WithSourceCodeInfo(LogSinks sinks, LogLevel level, string callerName, string sourcePath, int lineNum)
    {
        var message = "This is a {0} log event to {1}";

        var logger = GetLogger(sinks, LogLevel.Trace, ContextTemplate);
        logger.SourceCode().Log(level, message, level, sinks);

        if ((sinks & LogSinks.LastEvent) != LogSinks.LastEvent)
            return;

        LastEventSink.Should().NotBeNull();
        LastEventSink!.LastLogMessage.Should().NotBeNull();

        var sourceMessage = $"{message}\r\n{callerName}\r\n{sourcePath}:{lineNum}";
        var result = FormatTemplate(sourceMessage, level, level, sinks.ToString());
        LastEventSink.LastLogMessage!.Should().Be(result);
    }
}