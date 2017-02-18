//INSTANT C# NOTE: Formerly VB project-level imports:

using System.Collections.Generic;
using System.Linq;

namespace BigLamp.Extensions.Dictionary
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Returns a comma-separated list. Format: "key1=value=1, key2=value2"
        /// </summary>
        /// <param name="values"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToString(this Dictionary<string, string> values, string separator)
        {
            IEnumerable<string> items = values.Select(v => string.Format("{0}{1}{2}", v.Key, "=", v.Value));


            return string.Join(separator, items.ToArray());
        }
    }
}
