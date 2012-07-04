using System.Globalization;

namespace ArtSite.Extensions
{
    public static class StringExtensions
    {
         public static string ToTitleCase(this string text)
         {
             return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
         }

         public static bool IsNullOrWhiteSpace(this string text)
         {
             return string.IsNullOrWhiteSpace(text);
         }
    }
}