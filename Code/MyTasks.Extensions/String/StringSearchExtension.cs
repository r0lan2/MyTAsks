using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BigLamp.Extensions.String
{
    public static class StringSearchExtension
    {
        /// <summary>
        ///     Find first string match between a tag which can be a character or text.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="tag"></param>
        /// <returns>String without tags</returns>
        /// <remarks></remarks>
        public static string FindStringBetweenTags(this string value, string tag)
        {
            var match = Regex.Match(value, tag + "(.*?)" + tag);
            return match.Groups[1].Value.Replace(tag, string.Empty);
        }

        public static bool Contains(this string source, string value, StringComparison comparer)
        {
            if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(source))
            {
                return true;
            }
            return value != null && source.IndexOf(value, comparer) >= 0;
        }
        public static bool ContainsAll(this string source, params string[] values)
        {
            return values.All(source.Contains);
        }
    }
}