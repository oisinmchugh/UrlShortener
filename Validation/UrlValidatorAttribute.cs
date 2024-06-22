using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UrlShortener.Validation
{
    public class UrlValidatorAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || !(value is string url))
            {
                return false;
            }

            // Regular expression to validate a URL
            var regex = new Regex(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return regex.IsMatch(url);
        }
    }
}
