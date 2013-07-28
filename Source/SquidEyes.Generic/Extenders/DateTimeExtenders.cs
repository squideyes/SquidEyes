using System;
using System.Diagnostics;

namespace SquidEyes.Generic
{
    public static partial class DateAndTimeExtenders
    {
        private static TimeZoneInfo cstTzi;
        private static TimeZoneInfo estTzi;

        static DateAndTimeExtenders()
        {
            const string ESTTZNAME = "Eastern Standard Time";
            const string CSTTZNAME = "Central Standard Time";

            cstTzi = TimeZoneInfo.FindSystemTimeZoneById(CSTTZNAME);
            estTzi = TimeZoneInfo.FindSystemTimeZoneById(ESTTZNAME);
        }

        public static DateTime ToCST(this DateTime dateTime)
        {
            Debug.Assert(dateTime.Kind == DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTime(dateTime, cstTzi);
        }

        public static DateTime ToEST(this DateTime dateTime)
        {
            Debug.Assert(dateTime.Kind == DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTime(dateTime, estTzi);
        }
    }
}
