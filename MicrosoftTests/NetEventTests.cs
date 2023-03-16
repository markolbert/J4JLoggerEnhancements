#region license

// Copyright 2021 Mark A. Olbert
// 
// This library or program 'J4JLoggingTests' is free software: you can redistribute it
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

using System.ComponentModel;
using FluentAssertions;
using J4JSoftware.Logging;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace MicrosoftTests
{
    public class NetEventTests : TestBase
    {
        private const string Message = "This is a net events test";

        private LogLevel _curLevel = LogLevel.Trace;
        private string _curAbbr = string.Empty;

        [ Theory ]
        [ InlineData( LogLevel.Information ) ]
        [ InlineData( LogLevel.Error ) ]
        [ InlineData( LogLevel.Debug ) ]
        [ InlineData( LogLevel.Critical ) ]
        [ InlineData( LogLevel.Warning ) ]
        [ InlineData( LogLevel.Trace) ]
        public void TestEvent( LogLevel level )
        {
            var logger = GetLogger(LogSinks.NetEvent, LogLevel.Trace, NoContextTemplate);

            _curLevel = level;

            _curAbbr = level switch
            {
                LogLevel.Debug => "DBG",
                LogLevel.Error => "ERR",
                LogLevel.Critical => "FTL",
                LogLevel.Information => "INF",
                LogLevel.Trace => "VRB",
                LogLevel.Warning => "WRN",
                _ => throw new
                    InvalidEnumArgumentException($"Unsupported {nameof(LogEventLevel)} '{level}'")
            };


            logger.Log(level, Message);
        }

        protected override void OnNetEvent( NetEventArgs e )
        {
            base.OnNetEvent( e );

            e.LogEvent.Level.Should().Be( _curLevel.ToSerilogLevel() );
            e.LogMessage.Should().Be( $"{_curAbbr} {Message}" );
        }
    }
}
