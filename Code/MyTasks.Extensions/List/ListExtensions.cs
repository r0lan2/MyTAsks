//INSTANT C# NOTE: Formerly VB project-level imports:

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BigLamp.Extensions.List
{
    public static class ListExtensions
    {

        public static HashSet<T> ToHashSet<T>(this List<T> list)
        {
            var result = new HashSet<T>();
            foreach (var item in list)
            {
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Returns a delimited string of the strings in the list. Comma is default
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToDelimitedSeparatedString<T>(this List<T> list, string delimiter = ",")
        {
            if (list.Count > 0)
            {
                return list.Select(s => s.ToString()).Aggregate((first, second) => (first + delimiter) + second);
            }
            return string.Empty;
        }
        public static string ToDelimitedSeparatedString(this List<double> list, System.IFormatProvider provider,string delimiter = ",")
        {
            if (list.Count > 0)
            {
                return list.Select(s => s.ToString(provider)).Aggregate((first, second) => (first + delimiter) + second);
            }
            return string.Empty;
        }
    }
}
