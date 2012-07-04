using System.Globalization;

namespace ArtSite.Extensions
{
    public static class StringExtensions
    {
         public static string ToTitleCase(this string text)
         {
             return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
         }
    }
}