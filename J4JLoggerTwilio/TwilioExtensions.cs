﻿// Copyright (c) 2021, 2022 Mark A. Olbert 
// 
// This file is part of J4JLogger.
//
// J4JLogger is free software: you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the 
// Free Software Foundation, either version 3 of the License, or 
// (at your option) any later version.
// 
// J4JLogger is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
// 
// You should have received a copy of the GNU General Public License along 
// with J4JLogger. If not, see <https://www.gnu.org/licenses/>.

using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Twilio;

namespace J4JSoftware.Logging;

public static class TwilioExtensions
{
    public static LoggerConfiguration Twilio(
        this LoggerSinkConfiguration sinkConfig,
        TwilioConfiguration configValues,
        TemplateElements templateElements = TemplateElements.Sms,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose
    )
    {
        if (!configValues.IsValid)
            throw new ArgumentException("Twilio configuration values are invalid");

        TwilioClient.Init(configValues.AccountSID!, configValues.AccountToken!);

        var sink = new TwilioSink(configValues.FromNumber!,
            configValues.Recipients!,
            templateElements.GetTemplate())
        {
            IsConfigured = true
        };

        return sinkConfig.Sink(sink, restrictedToMinimumLevel);
    }
}