using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;

namespace PhotoAlbum
{

    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string s1, string s2) => s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase);

        public static bool MatchesAny(this string s, IEnumerable<Regex> regexes) => regexes.Any(r => r.IsMatch(s));

        public static bool EqualsAny(this string s1, IEnumerable<string> strings, bool ignoreCase = false) => ignoreCase
            ? strings.Any(s2 => s2.EqualsIgnoreCase(s1))
            : strings.Any(s2 => s2.Equals(s1));

        public static string UrlDecode(this string encoded) => WebUtility.UrlDecode(encoded);

        public static string UrlEncode(this string decoded) => WebUtility.UrlEncode(decoded);
    }
}