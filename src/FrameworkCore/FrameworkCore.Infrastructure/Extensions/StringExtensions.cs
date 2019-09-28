using System.Text.RegularExpressions;

namespace FrameworkCore.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ConvertToUrlFriendly(this string input)
        {
            return input.ConvertToNonAscii()
                .RemoveAllNonAlphanumericCharacters()
                .ConvertToSingleSpace()
                .Replace(" ", "-");
        }

        public static string ConvertToNonAscii(this string input)
        {
            return Regex.Replace(input, @"[^\u0000-\u007F]+", string.Empty);
        }

        public static string RemoveAllNonAlphanumericCharacters(this string input)
        {
            return Regex.Replace(input, "[^a-zA-Z0-9 -]", string.Empty);
        }

        public static string ConvertToSingleSpace(this string input)
        {
            return Regex.Replace(input, @"\s+", " ");
        }


    }
}
