#region license

// Copyright 2021 Mark A. Olbert
// 
// This library or program 'J4JLoggingEnhancementTests' is free software: you can redistribute it
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
using Serilog;
using Serilog.Events;

namespace J4JLoggingEnhancementTests
{
    public class LoggingTests : TestBase
    {
        [Theory]
        [InlineData(LogSinks.Debug | LogSinks.LastEvent, LogEventLevel.Verbose)]
        public void Uncached(LogSinks sinks, LogEventLevel level)
        {
            var template = "This is a {0} log event to {1}";

            var logger = GetLogger(sinks, LogEventLevel.Verbose);
            logger.Write(level, template, level, sinks.ToString());

            if ((sinks & LogSinks.LastEvent) != LogSinks.LastEvent)
                return;

            LastEvent.Should().NotBeNull();
            LastEvent!.LastLogMessage.Should().NotBeNull();
            LastEvent.LastLogMessage!.Should().Be(FormatTemplate(template, level, sinks.ToString()));
        }

        //[ Fact ]
        //public void Cached()
        //{
        //    var cached = new J4JCachedLogger();
        //    cached.SetLoggedType( GetType() );

        //    var template = "{0} (test message)";

        //    cached.Verbose<string>( template, "Verbose" );
        //    cached.Warning<string>( template, "Warning" );
        //    cached.Information<string>( template, "Information" );
        //    cached.Debug<string>( template, "Debug" );
        //    cached.Error<string>( template, "Error" );
        //    cached.Fatal<string>( template, "Fatal" );

        //    cached.SmsHandling = SmsHandling.SendNextMessage;
        //    cached.Verbose<string>( "{0} (test message)", "Verbose" );

        //    foreach( var entry in cached.Entries )
        //    {
        //        var logger = entry.SmsHandling != SmsHandling.DoNotSend ? Logger.SendToSms() : Logger;

        //        Logger.Write( entry.LogEventLevel,
        //                     entry.MessageTemplate,
        //                     entry.PropertyValues,
        //                     entry.MemberName,
        //                     entry.SourcePath,
        //                     entry.SourceLine );

        //        LastEvent.LastLogMessage.Should().Be( FormatMessage( entry.LogEventLevel.ToString() ) );
        //    }

        //    string FormatMessage( string prop1 )
        //    {
        //        return template.Replace( "{0}", $"\"{prop1}\"" );
        //    }
        //}
    }
}
