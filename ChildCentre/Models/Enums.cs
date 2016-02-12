using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChildCentre.Models
{
    public struct Global
    {
        public static TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
    }
}