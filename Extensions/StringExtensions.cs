using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
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

        public static byte[] GetBytes(this string source) => Encoding.UTF8.GetBytes(source);

        public static string HashMD5(this string source)
        {
            if (string.IsNullOrWhiteSpace(source)) return string.Empty;
            using var md5 = MD5.Create();
            var hashData = md5.ComputeHash(source.GetBytes());
            var hash = Convert.ToBase64String(hashData);
            return hash.Replace("=", "");
        }
    }
}