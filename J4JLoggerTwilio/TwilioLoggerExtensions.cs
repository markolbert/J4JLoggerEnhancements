﻿#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// TwilioLoggerExtensions.cs
//
// This file is part of JumpForJoy Software's J4JLoggerTwilio.
// 
// J4JLoggerTwilio is free software: you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the 
// Free Software Foundation, either version 3 of the License, or 
// (at your option) any later version.
// 
// J4JLoggerTwilio is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
// 
// You should have received a copy of the GNU General Public License along 
// with J4JLoggerTwilio. If not, see <https://www.gnu.org/licenses/>.
#endregion

using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting;
using Twilio;

namespace J4JSoftware.Logging;

public static class TwilioLoggerExtensions
{
    public static LoggerConfiguration Twilio(
        this LoggerSinkConfiguration sinkConfig,
        TwilioConfiguration configValues,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose,
        string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    {
        if (!configValues.IsValid)
            throw new ArgumentException("Twilio configuration values are invalid");

        TwilioClient.Init(configValues.AccountSid!, configValues.AccountToken!);

        var sink = new TwilioSink(outputTemplate, configValues.FromNumber!, configValues.Recipients!)
        {
            IsConfigured = true
        };

        return sinkConfig.Sink(sink, restrictedToMinimumLevel);
    }
    public static LoggerConfiguration Twilio(
        this LoggerSinkConfiguration sinkConfig,
        ITextFormatter formatter,
        TwilioConfiguration configValues,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose
    )
    {
        if (!configValues.IsValid)
            throw new ArgumentException("Twilio configuration values are invalid");

        TwilioClient.Init(configValues.AccountSid!, configValues.AccountToken!);

        var sink = new TwilioSink(formatter, configValues.FromNumber!, configValues.Recipients!)
        {
            IsConfigured = true
        };

        return sinkConfig.Sink(sink, restrictedToMinimumLevel);
    }

}