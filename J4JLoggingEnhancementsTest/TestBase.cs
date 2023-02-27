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
using Microsoft.Extensions.Configuration;
using Serilog;

namespace J4JLoggingEnhancementTests
{
    public class TestBase
    {
        protected TestBase()
        {
            var configBuilder = new ConfigurationBuilder();

            var config = configBuilder
                         .AddUserSecrets<LoggingTests>()
                         .Build();

            var twilioConfig = config.Get<TwilioConfiguration>();
            twilioConfig.Should().NotBeNull();

            var loggerConfig = new LoggerConfiguration()
                .WriteTo.Debug()
                .WriteTo.Twilio(twilioConfig!)
                .WriteTo.LastEvent(out var temp)
                .WriteTo.NetEvent();

            LastEvent = temp!;

            Logger = loggerConfig.CreateLogger();
        }

        private void LogEvent( object? sender, NetEventArgs e ) => OnNetEvent( e );

        protected virtual void OnNetEvent( NetEventArgs e )
        {
        }

        protected ILogger Logger { get; }
        protected LastEventSink LastEvent { get; }
    }
}
