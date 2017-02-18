//INSTANT C# NOTE: Formerly VB project-level imports:

using System.Collections.Generic;
using System.Data;

namespace BigLamp.Extensions.Query
{
    public static class CustomLinqtoDataSetMethods
    {
        /// <summary>
        /// Copies query to a new datatable 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTable CopyQueryToDataTable<T>(this IEnumerable<T> source)
        {
            return (new ObjectShredder<T>()).Shred(source, null, null);
        }
        /// <summary>
        /// Copies query to a new datatable 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="table"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTable CopyQueryToDataTable<T>(this IEnumerable<T> source, DataTable table, LoadOption? options)
        {
            return (new ObjectShredder<T>()).Shred(source, table, options);
        }


    }
}