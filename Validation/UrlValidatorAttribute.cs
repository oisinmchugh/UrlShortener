﻿namespace UrlShortener.Validation
{
    using System.ComponentModel.DataAnnotations;

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

            try
            {
                var uri = new Uri(url);
                if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                {
                    return new ValidationResult("Invalid URL format.");
                }
            }
            catch
            {
                return new ValidationResult("Invalid URL format.");
            }

            return ValidationResult.Success;
        }
    }
}