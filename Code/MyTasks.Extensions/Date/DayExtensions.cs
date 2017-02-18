//INSTANT C# NOTE: Formerly VB project-level imports:

using System;
using System.Collections.Generic;
using System.Globalization;
using BigLamp.Extensions.String;
using Microsoft.VisualBasic;

namespace BigLamp.Extensions.Date
{
    public static class DayExtensions
    {

        /// <summary>
        /// Gets a DateTime representing the first day in the current week (ISO Week)
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime FirstDayOfWeek(this DateTime current)
        {
            return current.AddDays(1 - DateAndTime.DatePart(DateInterval.Weekday, current, Constants.vbMonday));
        }

        /// <summary>
        /// Gets a DateTime representing the first day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime current)
        {
            return current.AddDays(1 - current.Day);
        }

        /// <summary>
        /// Gets a DateTime representing the first specified day in the current month
        /// </summary>
        /// <param name="current">The current day</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime first = current.FirstDayOfMonth();
            if (first.DayOfWeek != dayOfWeek)
            {
                first = first.NextDay(dayOfWeek);
            }
            return first;
        }


        /// <summary>
        /// Gets a DateTime representing the first day in the previous month
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime FirstDayOfPreviousMonth(this DateTime current)
        {
            return current.FirstDayOfMonth().AddDays(-1).FirstDayOfMonth();
        }

        /// <summary>
        /// Gets a DateTime representing the first specified day in the previous month
        /// </summary>
        /// <param name="current"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime FirstDayOfPreviousMonth(this DateTime current, DayOfWeek dayOfWeek)
        {
            return current.FirstDayOfMonth().AddDays(-1).FirstDayOfMonth(dayOfWeek);
        }
        /// <summary>
        /// Gets a DateTime representing the last day in the current week (ISO Week)
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime LastDayOfWeek(this DateTime current)
        {
            return current.FirstDayOfWeek().AddDays(6);
        }
        /// <summary>
        /// Gets a DateTime representing the first day in the previous week (ISO Week)
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime FirstDayOfPreviousWeek(this DateTime current)
        {
            return current.FirstDayOfWeek().AddDays(-1).FirstDayOfWeek();
        }
        /// <summary>
        /// Gets a DateTime representing the last day in the previous week (ISO Week)
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime LastDayOfPreviousWeek(this DateTime current)
        {
            return current.FirstDayOfWeek().AddDays(-1);
        }
        /// <summary>
        /// Gets a DateTime representing the last day in the previous month
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime LastDayOfPreviousMonth(this DateTime current)
        {
            return current.FirstDayOfMonth().AddDays(-1);
        }
        /// <summary>
        /// Gets a DateTime representing the last specified day in the previous month
        /// </summary>
        /// <param name="current"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime LastDayOfPreviousMonth(this DateTime current, DayOfWeek dayOfWeek)
        {
            return current.FirstDayOfMonth().AddDays(-1).LastDayOfMonth(dayOfWeek);
        }
        /// <summary>
        /// Gets a DateTime representing the last day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime current)
        {
            int daysInMonth = DateTime.DaysInMonth(current.Year, current.Month);
            return current.FirstDayOfMonth().AddDays(daysInMonth - 1);
        }

        /// <summary>
        /// Gets a DateTime representing the last specified day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime last = current.LastDayOfMonth();
            return last.AddDays(Math.Abs(dayOfWeek - last.DayOfWeek) * -1);
        }

        /// <summary>
        /// Gets a DateTime representing the first date following the current date which falls on the given day of the week
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The day of week for the next date to get</param>
        public static DateTime NextDay(this DateTime current, DayOfWeek dayOfWeek)
        {
            int offsetDays = dayOfWeek - current.DayOfWeek;
            if (offsetDays <= 0)
            {
                offsetDays += 7;
            }
            return current.AddDays(offsetDays);
        }
        /// <summary>
        /// Gets a list of dates in specified range
        /// </summary>
        /// <param name="current"></param>
        /// <param name="fromDate"></param>
        /// <returns>List of Date</returns>
        /// <remarks></remarks>
        public static List<DateTime> GetElapasedDaysInPeriod(this DateTime current, DateTime fromDate)
        {
            var days = new List<DateTime>();
            while (current >= fromDate)
            {
                days.Add(current);
                current = current.AddDays(-1);
            }
            return days;
        }
        /// <summary>
        /// Gets a list of dates from specified number of months to current date. First Date is first day of first month.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="monthCount"></param>
        /// <returns>List of Date</returns>
        /// <remarks></remarks>
        public static List<DateTime> GetElapasedDaysInPeriod(this DateTime current, int monthCount)
        {
            if (monthCount > 1)
            {
                var fromdate = current.FirstDayOfMonth();
                fromdate = fromdate.AddMonths((monthCount - 1) * -1);
                return GetElapasedDaysInPeriod(current, fromdate);
            }
            return current.GetElapasedDaysInCurrentMonth();
        }

        /// <summary>
        /// Gets a list of dates from first of current month to current date.
        /// </summary>
        /// <param name="current"></param>
        /// <returns>List of Date</returns>
        /// <remarks></remarks>
        public static List<DateTime> GetElapasedDaysInCurrentMonth(this DateTime current)
        {
            return current.GetElapasedDaysInPeriod(current.FirstDayOfMonth());
        }
        /// <summary>
        /// Gets a list of months in timespan starting with today, with latest month first in the list.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Dictionary<string, DateTime> GetMonthList(this DateTime current)
        {
            return GetMonthList(current, DateTime.Today);
        }
        /// <summary>
        /// Gets a list of months in timespan with latest month first in the list.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Dictionary<string, DateTime> GetMonthList(this DateTime current, DateTime toDate)
        {
            var months = new Dictionary<string, DateTime>();

            current = current.FirstDayOfMonth();

            while (toDate >= current)
            {
                string monthText = string.Format("{0} {1}", toDate.FirstDayOfMonth().ToString("MMMM").ToProperCase(), toDate.FirstDayOfMonth().Year.ToString(CultureInfo.InvariantCulture));
                months.Add(monthText, toDate.FirstDayOfMonth());
                toDate = toDate.FirstDayOfPreviousMonth();
            }

            return months;
        }
        /// <summary>
        /// MonthNumber from 1990-01-01
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int MonthCountFrom1990(this DateTime current)
        {
            var startDate = new DateTime(1990, 1, 1);

            return Convert.ToInt32(DateAndTime.DateDiff(DateInterval.Month, startDate, current, Microsoft.VisualBasic.FirstDayOfWeek.Monday)) + 1;

        }
        /// <summary>
        /// ISO 8601 Week number of the specified date. 
        /// ISO 8601 defines the first week of the year as the one that includes the first Thursday. The weeks are numbered starting with 1 through to 52 or 53.
        /// This presumes that weeks start with Monday. 
        /// </summary>
        /// <returns>Week number of the specified date</returns>
        /// <remarks>"ISO 8601 Week of Year format in Microsoft .Net" by Shawn Steele: http://blogs.msdn.com/shawnste/archive/2006/01/24/iso-8601-week-of-year-format-in-microsoft-net.aspx</remarks>
        public static int ISOWeek(this DateTime current)
        {
            // Need a calendar.  Culture's irrelevent since we specify start day of week
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = calendar.GetDayOfWeek(current);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                current = current.AddDays(3);
            }
            // Return the week of our adjusted day
            int weekNumber = calendar.GetWeekOfYear(current, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }


        /// <summary>
        /// Gets a list of months in timespan with latest month first in the list.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="toDate"></param>
        /// <param name="weekText"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Dictionary<string, DateTime> GetWeekList(this DateTime current, DateTime toDate, string weekText = "Week")
        {
            var weeks = new Dictionary<string, DateTime>();

            current = current.FirstDayOfWeek();

            while (toDate >= current)
            {
                string monthText = string.Format("{0} {1} [{2}]", weekText, toDate.FirstDayOfWeek().ISOWeek().ToString(CultureInfo.InvariantCulture), toDate.FirstDayOfWeek().ToStringAsIsoDate());
                weeks.Add(monthText, toDate.FirstDayOfWeek());
                toDate = toDate.FirstDayOfWeek().AddDays(-1).FirstDayOfWeek();
            }

            return weeks;
        }

        public static DateTime AddWeeks(this DateTime current, int numberOfWeeksToAdd)
        {
            return current.AddDays(numberOfWeeksToAdd * 7);
        }


        /// <summary>
        /// Gets a DateTime representing the first day in the current quarter
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime FirstDayOfQuarter(this DateTime current)
        {
            DateTime first = current.AddMonths(FirstMonthOfQuarter(CurrentQuarter(current)) - current.Month);
            return first.FirstDayOfMonth();
        }
        /// <summary>
        /// Gets a DateTime representing the first day in the previous quarter
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime FirstDayOfPreviousQuarter(this DateTime current)
        {
            return current.FirstDayOfQuarter().AddDays(-1).FirstDayOfQuarter();
        }

        /// <summary>
        /// Gets a DateTime representing the first day in the current year
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime FirstDayOfYear(this DateTime current)
        {
            DateTime first = current.AddMonths(1 - current.Month);
            return first.FirstDayOfMonth();
        }
        /// <summary>
        /// Gets a DateTime representing the first day in the current year
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime FirstDayOfPreviousYear(this DateTime current)
        {
            return current.FirstDayOfYear().AddDays(-1).FirstDayOfYear();
        }
        /// <summary>
        /// Gets a DateTime representing the last day in the current quarter
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime LastDayOfQuarter(this DateTime current)
        {
            DateTime last = current.AddMonths(LastMonthOfQuarter(CurrentQuarter(current)) - current.Month);
            return last.LastDayOfMonth();
        }

        /// <summary>
        /// Gets a DateTime representing the last day in the previous quarter
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime LastDayOfPreviousQuarter(this DateTime current)
        {
            return current.FirstDayOfQuarter().AddDays(-1);
        }
        /// <summary>
        /// Gets a DateTime representing the last day in the current year
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime LastDayOfYear(this DateTime current)
        {
            DateTime last = current.AddMonths(12 - current.Month);
            return last.LastDayOfMonth();
        }
        /// <summary>
        /// Gets a DateTime representing the last day in the current year
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime LastDayOfPreviousYear(this DateTime current)
        {
            return current.FirstDayOfYear().AddDays(-1);
        }
        private static int FirstMonthOfQuarter(int quarter)
        {
            switch (quarter)
            {
                case 1:
                    return 1;
                case 2:
                    return 4;
                case 3:
                    return 7;
                case 4:
                    return 10;
            }
            return 1;
        }
        private static int LastMonthOfQuarter(int quarter)
        {
            switch (quarter)
            {
                case 1:
                    return 3;
                case 2:
                    return 6;
                case 3:
                    return 9;
                case 4:
                    return 12;
            }
            return 1;
        }
        private static int CurrentQuarter(DateTime current)
        {
            switch (current.Month)
            {
                case 1:
                case 2:
                case 3:
                    return 1;
                case 4:
                case 5:
                case 6:
                    return 2;
                case 7:
                case 8:
                case 9:
                    return 3;
                case 10:
                case 11:
                case 12:
                    return 4;
            }
            return 1;
        }
    }
}
