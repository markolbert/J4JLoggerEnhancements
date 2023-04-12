#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// TwilioConfiguration.cs
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

using System.Collections.Generic;
using System.Linq;

namespace J4JSoftware.Logging
{
    public class TwilioConfiguration
    {
        public string? AccountSid { get; set; }
        public string? AccountToken { get; set; }
        public string? FromNumber { get; set; }
        public List<string>? Recipients { get; set; }

        public bool IsValid =>
            !string.IsNullOrEmpty( AccountSid )
            && !string.IsNullOrEmpty( AccountToken )
            && !string.IsNullOrEmpty( FromNumber )
            && ( Recipients?.Any() ?? false );
    }
}
