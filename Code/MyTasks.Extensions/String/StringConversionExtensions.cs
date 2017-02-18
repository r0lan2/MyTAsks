//INSTANT C# NOTE: Formerly VB project-level imports:

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BigLamp.Extensions.String
{
    public static class StringConversionExtensions
    {
        /// <summary>
        /// First letter of whole string is set to capital letter
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToProperCase(this string s)
        {
            string pString = s.ToLower();
            if (!(string.IsNullOrEmpty(pString)))
            {
                pString = pString.Substring(0, 1).ToUpper() + pString.Substring(1, pString.Length - 1);
            }
            return pString;
        }

        /// <summary>
        /// First letter of whole string is set to lower letter
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToCamelCase(this string s)
        {
            string pString = s;
            if (!(string.IsNullOrEmpty(pString)))
            {
                pString = pString.Substring(0, 1).ToLowerInvariant() + pString.Substring(1, pString.Length - 1);
            }
            return pString;
        }

        /// <summary>
        /// Returns the numbers from the string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetNumbersFromString(this string s)
        {
            Match matchObj = Regex.Match(s, "[0-9]+");
            string newString = "";
            while (matchObj.Success)
            {
                newString += matchObj.Value;
                matchObj = matchObj.NextMatch();
            }
            return newString;
        }
        /// <summary>
        /// Counts number of occurencies of a word in a string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="wordToFind"></param>
        /// <returns></returns>
        public static int ContainsCount(this string s, string wordToFind)
        {
            return (s.Length - s.Replace(wordToFind, "").Length) / wordToFind.Length;
        }

        private static string _paraBreak = "\r\n\r\n";
        private static string _link = "<a href=\"{0}\">{1}</a>";
        private static string _linkNoFollow = "<a href=\"{0}\" rel=\"nofollow\">{1}</a>";

        /// <summary>
        /// Returns a copy of this string converted to HTML markup.
        /// </summary>
        public static string ToHtml(this string s)
        {
            return ToHtml(s, false);
        }

        /// <summary>
        /// Returns a copy of this string converted to HTML markup.
        /// 
        /// Source: http://www.blackbeltcoder.com/Articles/strings/converting-text-to-html
        /// </summary>
        /// <param name="s"></param>
        /// <param name="nofollow">If true, links are given "nofollow"
        /// attribute</param>
        public static string ToHtml(this string s, bool nofollow)
        {
            //Listing 1 shows my ToHtml() extension method. This code adds the ToHtml() method to string variables.
            //
            //The ToHtml() method converts blocks of text separated by two or more newlines into paragraphs (using <p></p> tags). It converts single newlines into line breaks (using <br> tags). 
            //And it calls HttpUtility.HtmlEncode() to HTML-encode special characters.
            //
            //In addition, this method supports a special syntax for specifying links. Because regular <a> tags would be encoded by this method, 
            //a special syntax is required to allow users to specify a hyperlink.
            //
            // This syntax uses double square brackets ([[ and ]]). So, for example [[http://www.blackbeltcoder.com]] produces a hyperlink with http://www.blackbeltcoder.com as both the anchor text and the target URL.
            //
            //You can also specify two text values in the form [[Black Belt Coder][http://www.blackbeltcoder.com]]. This produces a hyperlink with Black Belt Coder as the 
            //anchor text and http://www.blackbeltcoder.com as the target URL.
            //
            //If you are simply taking unformatted text and displaying it on a Web page, then this special link syntax won't come into play 
            //(the double square brackets are unlikely to occur naturally). But if you or your users want the ability to submit Web content in plain text, this provides an easy syntax to specify hyperlinks.
       
            var sb = new StringBuilder();

            int pos = 0;
            while (pos < s.Length)
            {
                // Extract next paragraph
                int start = pos;
                pos = s.IndexOf(_paraBreak, start);
                if (pos < 0)
                    pos = s.Length;
                string para = s.Substring(start, pos - start).Trim();

                // Encode non-empty paragraph
                if (para.Length > 0)
                    EncodeParagraph(para, sb, nofollow);

                // Skip over paragraph break
                pos += _paraBreak.Length;
            }
            // Return result
            return sb.ToString();
        }

        /// <summary>
        /// Encodes a single paragraph to HTML.
        /// </summary>
        /// <param name="s">Text to encode</param>
        /// <param name="sb">StringBuilder to write results</param>
        /// <param name="nofollow">If true, links are given "nofollow"
        /// attribute</param>
        private static void EncodeParagraph(string s, StringBuilder sb, bool nofollow)
        {
            // Start new paragraph
            sb.AppendLine("<p>");

            // HTML encode text
            s = HttpUtility.HtmlEncode(s);

            // Convert single newlines to <br>
            s = s.Replace(Environment.NewLine, "<br />\r\n");

            // Encode any hyperlinks
            EncodeLinks(s, sb, nofollow);

            // Close paragraph
            sb.AppendLine("\r\n</p>");
        }

        /// <summary>
        /// Encodes [[URL]] and [[Text][URL]] links to HTML.
        /// </summary>
        /// <param name="text">Text to encode</param>
        /// <param name="sb">StringBuilder to write results</param>
        /// <param name="nofollow">If true, links are given "nofollow"
        /// attribute</param>
        private static void EncodeLinks(string text, StringBuilder sb, bool nofollow)
        {
            // Parse and encode any hyperlinks
            int pos = 0;
            while (pos < text.Length)
            {
                // Look for next link
                int start = pos;
                pos = text.IndexOf("[[", pos);
                if (pos < 0)
                    pos = text.Length;
                // Copy text before link
                sb.Append(text.Substring(start, pos - start));
                if (pos < text.Length)
                {
                    string label, link;

                    start = pos + 2;
                    pos = text.IndexOf("]]", start);
                    if (pos < 0)
                        pos = text.Length;
                    label = text.Substring(start, pos - start);
                    int i = label.IndexOf("][");
                    if (i >= 0)
                    {
                        link = label.Substring(i + 2);
                        label = label.Substring(0, i);
                    }
                    else
                    {
                        link = label;
                    }
                    // Append link
                    sb.Append(string.Format(nofollow ? _linkNoFollow : _link, link, label));

                    // Skip over closing "]]"
                    pos += 2;
                }
            }
        }
        public static string[] SplitCamelCase(this string source)
        {
            return Regex.Split(source, @"(?<!^)(?=[A-Z])");
        }
    }
}
