#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// NetEventSink.cs
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

namespace J4JSoftware.Logging;

public class NetEventSink : ILogEventSink
{
    public event EventHandler<NetEventArgs>? LogEvent;

    private readonly StringBuilder _sb = new();
    private readonly StringWriter _stringWriter;
    private readonly ITextFormatter _textFormatter;

    public NetEventSink( string outputTemplate )
    {
        _stringWriter = new StringWriter( _sb );
        _textFormatter = new MessageTemplateTextFormatter( outputTemplate );
    }

    public NetEventSink(ITextFormatter textFormatter)
    {
        _stringWriter = new StringWriter(_sb);
        _textFormatter = textFormatter;
    }

    public void Emit( LogEvent logEvent )
    {
        _sb.Clear();
        _textFormatter.Format( logEvent, _stringWriter );
        _stringWriter.Flush();

        LogEvent?.Invoke( this, new NetEventArgs( logEvent, _sb.ToString() ) );
    }
}