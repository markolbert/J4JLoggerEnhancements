#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// SmsHandling.cs
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

namespace J4JSoftware.Logging;

public enum SmsHandling
{
    DoNotSend,
    SendNextMessage,
    SendUntilReset
}