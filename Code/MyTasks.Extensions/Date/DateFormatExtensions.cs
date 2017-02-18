//INSTANT C# NOTE: Formerly VB project-level imports:

using System;

namespace BigLamp.Extensions.Date
{
    public static class DateFormatExtensions
    {

        /// <summary>
        /// Converts the datetime object to a string with format yyyy-MM-dd.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToStringAsIsoDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Converts the datetime object to a string with format yyyy-MM-dd 00:00:00.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToStringAsIsoDateTime(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Exlude milliseconds from date.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime WithoutMilliseconds(this DateTime dt)
        {
            return dt.AddMilliseconds(-dt.Millisecond);
        }
    }
}
