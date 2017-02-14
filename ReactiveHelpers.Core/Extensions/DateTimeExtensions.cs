using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactiveHelpers
{
    public static class DateTimeExtensions
    {
        public static readonly DateTime BeginOfEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts a nullable date/time value to UTC.
        /// </summary>
        /// <param name="dateTime">The nullable date/time</param>
        /// <returns>The nullable date/time in UTC</returns>
        public static DateTime? ToUniversalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToUniversalTime() : (DateTime?)null;
        }

        /// <summary>
        /// Converts a nullable UTC date/time value to local time.
        /// </summary>
        /// <param name="dateTime">The nullable UTC date/time</param>
        /// <returns>The nullable UTC date/time as local time</returns>
        public static DateTime? ToLocalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToLocalTime() : (DateTime?)null;
        }


        /// <summary>
        /// Returns a date that is rounded to the next even hour above the given
        /// date.
        /// <p>
        /// For example an input date with a time of 08:13:54 would result in a date
        /// with the time of 09:00:00. If the date's time is in the 23rd hour, the
        /// date's 'day' will be promoted, and the time will be set to 00:00:00.
        /// </p>
        /// </summary>
        /// <param name="dateTime">the Date to round, if <see langword="null" /> the current time will
        /// be used</param>
        /// <returns>the new rounded date</returns>
        public static DateTime GetEvenHourDate(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }
            DateTime d = dateTime.Value.AddHours(1);
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0);
        }

        /// <summary>
        /// Returns a date that is rounded to the next even minute above the given
        /// date.
        /// <p>
        /// For example an input date with a time of 08:13:54 would result in a date
        /// with the time of 08:14:00. If the date's time is in the 59th minute,
        /// then the hour (and possibly the day) will be promoted.
        /// </p>
        /// </summary>
        /// <param name="dateTime">The Date to round, if <see langword="null" /> the current time will  be used</param>
        /// <returns>The new rounded date</returns>
        public static DateTime GetEvenMinuteDate(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }

            DateTime d = dateTime.Value;
            d = d.AddMinutes(1);
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
        }

        /// <summary>
        /// Returns a date that is rounded to the previous even minute below the
        /// given date.
        /// <p>
        /// For example an input date with a time of 08:13:54 would result in a date
        /// with the time of 08:13:00.
        /// </p>
        /// </summary>
        /// <param name="dateTime">the Date to round, if <see langword="null" /> the current time will
        /// be used</param>
        /// <returns>the new rounded date</returns>
        public static DateTime GetEvenMinuteDateBefore(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }

            DateTime d = dateTime.Value;
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
        }

        public static long ToJavaScriptTicks(this DateTime dateTime)
        {
            DateTimeOffset utcDateTime = dateTime.ToUniversalTime();
            long javaScriptTicks = (utcDateTime.Ticks - BeginOfEpoch.Ticks) / (long)10000;
            return javaScriptTicks;
        }

        /// <summary>
        /// Get the first day of the month for
        /// any full date submitted
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            DateTime dtFrom = date;
            dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));
            return dtFrom;
        }

        /// <summary>
        /// Get the last day of the month for any
        /// full date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            DateTime dtTo = date;
            dtTo = dtTo.AddMonths(1);
            dtTo = dtTo.AddDays(-(dtTo.Day));
            return dtTo;
        }

        public static DateTime ToEndOfTheDay(this DateTime dt)
        {
            if (dt != null)
                return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            return dt;
        }

        public static DateTime? ToEndOfTheDay(this DateTime? dt)
        {
            return (dt.HasValue ? dt.Value.ToEndOfTheDay() : dt);
        }

        /// <summary>Epoch time. Number of seconds since midnight (UTC) on 1st January 1970.</summary>
        public static long ToUnixTime(this DateTime date)
        {
            return Convert.ToInt64((date.ToUniversalTime() - BeginOfEpoch).TotalSeconds);
        }

        /// <summary>UTC date based on number of seconds since midnight (UTC) on 1st January 1970.</summary>
        public static DateTime FromUnixTime(this long unixTime)
        {
            return BeginOfEpoch.AddSeconds(unixTime);
        }

#if CORE
        // Number of 100ns ticks per time unit 
        private const long TicksPerMillisecond = 10000;
        private const long TicksPerSecond = TicksPerMillisecond * 1000;
        private const long TicksPerMinute = TicksPerSecond * 60;
        private const long TicksPerHour = TicksPerMinute * 60;
        private const long TicksPerDay = TicksPerHour * 24;

        // Number of milliseconds per time unit 
        private const int MillisPerSecond = 1000;
        private const int MillisPerMinute = MillisPerSecond * 60;
        private const int MillisPerHour = MillisPerMinute * 60;
        private const int MillisPerDay = MillisPerHour * 24;

        // Number of days in a non-leap year 
        private const int DaysPerYear = 365;
        // Number of days in 4 years 
        private const int DaysPer4Years = DaysPerYear * 4 + 1;
        // Number of days in 100 years
        private const int DaysPer100Years = DaysPer4Years * 25 - 1;
        // Number of days in 400 years
        private const int DaysPer400Years = DaysPer100Years * 4 + 1;

        // Number of days from 1/1/0001 to 12/31/1600 
        private const int DaysTo1601 = DaysPer400Years * 4;
        // Number of days from 1/1/0001 to 12/30/1899 
        private const int DaysTo1899 = DaysPer400Years * 4 + DaysPer100Years * 3 - 367;
        // Number of days from 1/1/0001 to 12/31/9999
        private const int DaysTo10000 = DaysPer400Years * 25 - 366;

        internal const long MinTicks = 0;
        internal const long MaxTicks = DaysTo10000 * TicksPerDay - 1;
        private const long MaxMillis = (long)DaysTo10000 * MillisPerDay;

        private const long FileTimeOffset = DaysTo1601 * TicksPerDay;
        private const long DoubleDateOffset = DaysTo1899 * TicksPerDay;
        // The minimum OA date is 0100/01/01 (Note it's year 100).
        // The maximum OA date is 9999/12/31 
        private const long OADateMinAsTicks = (DaysPer100Years - DaysPerYear) * TicksPerDay;
        // All OA dates must be greater than (not >=) OADateMinAsDouble
        private const double OADateMinAsDouble = -657435.0;
        // All OA dates must be less than (not <=) OADateMaxAsDouble 
        private const double OADateMaxAsDouble = 2958466.0;

        private const int DatePartYear = 0;
        private const int DatePartDayOfYear = 1;
        private const int DatePartMonth = 2;
        private const int DatePartDay = 3;

        private static readonly int[] DaysToMonth365 = {
            0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365};
        private static readonly int[] DaysToMonth366 = {
            0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366};

        public static readonly DateTime MinValue = new DateTime(MinTicks, DateTimeKind.Unspecified);
        public static readonly DateTime MaxValue = new DateTime(MaxTicks, DateTimeKind.Unspecified);

        public static DateTime FromOADate(this double d)
        {
            return new DateTime(DoubleDateToTicks(d), DateTimeKind.Unspecified);
        }

        internal static long DoubleDateToTicks(double value)
        {
            if (value >= OADateMaxAsDouble || value <= OADateMinAsDouble)
                throw new ArgumentException("Arg_OleAutDateInvalid");
            long millis = (long)(value * MillisPerDay + (value >= 0 ? 0.5 : -0.5));
            // The interesting thing here is when you have a value like 12.5 it all positive 12 days and 12 hours from 01/01/1899
            // However if you a value of -12.25 it is minus 12 days but still positive 6 hours, almost as though you meant -11.75 all negative 
            // This line below fixes up the millis in the negative case
            if (millis < 0)
            {
                millis -= (millis % MillisPerDay) * 2;
            }

            millis += DoubleDateOffset / TicksPerMillisecond;

            if (millis < 0 || millis >= MaxMillis) throw new ArgumentException("Arg_OleAutDateScale");
            return millis * TicksPerMillisecond;
        }

        public static  double ToOADate(this DateTime d)
        {
            
            return TicksToOADate(d.Ticks);
        }

        private static double TicksToOADate(long value)
        {
            if (value == 0)
                return 0.0;  // Returns OleAut's zero'ed date value.
            if (value < TicksPerDay) // This is a fix for VB. They want the default day to be 1/1/0001 rathar then 12/30/1899.
                value += DoubleDateOffset; // We could have moved this fix down but we would like to keep the bounds check.
            if (value < OADateMinAsTicks)
                throw new OverflowException("Arg_OleAutDateInvalid");
            // Currently, our max date == OA's max date (12/31/9999), so we don't 
            // need an overflow check in that direction. 
            long millis = (value - DoubleDateOffset) / TicksPerMillisecond;
            if (millis < 0)
            {
                long frac = millis % MillisPerDay;
                if (frac != 0) millis -= (MillisPerDay + frac) * 2;
            }
            return (double)millis / MillisPerDay;
        }
#endif
    }


}
