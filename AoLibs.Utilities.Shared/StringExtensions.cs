using System.Text.RegularExpressions;

namespace AoLibs.Utilities.Shared
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            return input.Substring(0, 1).ToUpper() + input.Substring(1);
        }

        public static string FirstCharToLower(this string input)
        {
            return input.Substring(0, 1).ToLower() + input.Substring(1);
        }

        public static string Wrap(this string s, string start, string end)
        {
            return $"{start}{s}{end}";
        }

        public static string TrimWhitespaceInside(this string str, bool allWhitespce = true)
        {
            return Regex.Replace(str, (allWhitespce ? @"\s" : " ") + @"{2,}", " ");
        }
    }
}
