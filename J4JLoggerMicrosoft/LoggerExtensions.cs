#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// LoggerExtensions.cs
//
// This file is part of JumpForJoy Software's J4JLoggerMicrosoft.
// 
// J4JLoggerMicrosoft is free software: you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the 
// Free Software Foundation, either version 3 of the License, or 
// (at your option) any later version.
// 
// J4JLoggerMicrosoft is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
// 
// You should have received a copy of the GNU General Public License along 
// with J4JLoggerMicrosoft. If not, see <https://www.gnu.org/licenses/>.
#endregion

using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace J4JSoftware.Logging;

// thanx to codea for the basic idea used here to pass context info!
// https://stackoverflow.com/questions/70851808/net-core-3-1-set-forcontext-for-serilog-with-microsoft-ilogger-interface
public static class LoggerExtensions
{
    public static ILogger SourceCode(
        this ILogger logger,
        bool include = true,
        [ CallerMemberName ] string callerName = "",
        [ CallerFilePath ] string sourceFilePath = "",
        [ CallerLineNumber ] int sourceLineNumber = 0
    )
    {
        if( !include )
            return logger;

        var forContext = new Dictionary<string, object>
        {
            { LoggerTerms.CallerMemberElementName, callerName },
            { LoggerTerms.CallerPathElementName, sourceFilePath },
            { LoggerTerms.CallerLineNumElementName, sourceLineNumber },
        };

        logger.BeginScope( forContext );
        return logger;
    }

    public static ILogger SendToSms( this ILogger logger, bool send = true )
    {
        if( !send )
            return logger;

        var forContext = new Dictionary<string, object> { { LoggerTerms.SendToSmsElementName, true } };

        using( logger.BeginScope( forContext ) )
        {
            return logger;
        }
    }
}