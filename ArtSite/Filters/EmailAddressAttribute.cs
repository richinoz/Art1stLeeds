using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ArtSite.Filters
{
    public class EmailAddressAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var address = (string)value;
            if (string.IsNullOrEmpty(address))
            {
                return true;
            }
            var objNotNaturalPattern = new Regex(@"\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}\b");

            return objNotNaturalPattern.IsMatch(address);

        }
    }
}