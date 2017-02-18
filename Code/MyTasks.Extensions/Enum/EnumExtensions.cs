//INSTANT C# NOTE: Formerly VB project-level imports:

using System;
using System.Collections.Generic;
using System.Linq;

namespace BigLamp.Extensions.Enum
{
    public static class EnumExtensions
    {


        public static T SetFlag<T>(this System.Enum value, T flags)
        {
            //If Not value.[GetType]().IsEquivalentTo(GetType(T)) Then
            //    Throw New ArgumentException("Enum value and flags types don't match.")
            //End If

            // yes this is ugly, but unfortunately we need to use an intermediate boxing cast

            return (T)System.Enum.ToObject(typeof(T), Convert.ToUInt64(value) | Convert.ToUInt64(flags));
        }
        /// <summary>
        /// Gets a distinct list of duplicates
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IEnumerable<string> GetDuplicates(this IEnumerable<string> list)
        {
            var duplicated = list.GroupBy(i => i).Where(g => g.Count() > 1).Select(g => g.Key);
            return duplicated;
        }
        /// <summary>
        /// Gets a distinct list of duplicates
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IEnumerable<int> GetDuplicates(this IEnumerable<int> list)
        {
            var duplicated = list.GroupBy(i => i).Where(g => g.Count() > 1).Select(g => g.Key);
            return duplicated;
        }
        /// <summary>
        /// Gets a distinct list of duplicates
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IEnumerable<double> GetDuplicates(this IEnumerable<double> list)
        {
            var duplicated = list.GroupBy(i => i).Where(g => g.Count() > 1).Select(g => g.Key);
            return duplicated;
        }

    }
}
