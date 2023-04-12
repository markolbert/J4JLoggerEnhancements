#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// LastEventSink.cs
//
// This file is part of JumpForJoy Software's J4JLoggerSerilog.
// 
// J4JLoggerSerilog is free software: you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the 
// Free Software Foundation, either version 3 of the License, or 
// (at your option) any later version.
// 
// J4JLoggerSerilog is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
// 
// You should have received a copy of the GNU General Public License along 
// with J4JLoggerSerilog. If not, see <https://www.gnu.org/licenses/>.
#endregion

using System.Text;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

#pragma warning disable 8618

namespace J4JSoftware.Logging;

public class LastEventSink : ILogEventSink
{
    private readonly StringBuilder _sb = new();
    private readonly StringWriter _sw;
    private readonly ITextFormatter _formatter;

    public LastEventSink(
        string outputTemplate
    )
    {
        _formatter = new MessageTemplateTextFormatter(outputTemplate);
        _sw = new StringWriter(_sb);
    }

    public LastEventSink(
        ITextFormatter formatter
    )
    {
        _formatter = formatter;
        _sw = new StringWriter(_sb);
    }

    public LogEvent LastLogEvent { get; private set; }
    public string? LastLogMessage { get; private set; }

    public void Emit( LogEvent logEvent )
    {
        LastLogEvent = logEvent;

        _sb.Clear();
        _formatter.Format(logEvent, _sw);
        _sw.Flush();

        LastLogMessage = _sb.ToString();
    }
}