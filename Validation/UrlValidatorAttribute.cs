using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Validation
{
    public class UrlValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var url = value as string;
            if (string.IsNullOrEmpty(url))
            {
                return new ValidationResult("URL is required.");
            }

            // Validate the URL format
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return new ValidationResult("Invalid URL format.");
            }

            return ValidationResult.Success;
        }
    }
}