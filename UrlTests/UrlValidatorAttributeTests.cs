namespace UrlTests
{
    using System.ComponentModel.DataAnnotations;
    using UrlShortener.Validation;
    using NUnit.Framework;

    public class UrlValidatorTests
    {
        private UrlValidatorAttribute _urlValidator;

        [SetUp]
        public void Setup()
        {
            _urlValidator = new UrlValidatorAttribute();
        }

        [Test]
        public void IsValid_ShouldReturnSuccess_ForValidUrl()
        {
            // Arrange
            var validUrl = "https://www.example.com";

            // Act
            var result = _urlValidator.GetValidationResult(validUrl, new ValidationContext(new object()));

            // Assert
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        [Test]
        public void IsValid_ShouldReturnError_ForEmptyUrl()
        {
            // Arrange
            string emptyUrl = string.Empty;

            // Act
            var result = _urlValidator.GetValidationResult(emptyUrl, new ValidationContext(new object()));

            // Assert
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.EqualTo("URL is required."));
        }

        [Test]
        public void IsValid_ShouldReturnError_ForNullUrl()
        {
            // Arrange
            string nullUrl = null;

            // Act
            var result = _urlValidator.GetValidationResult(nullUrl, new ValidationContext(new object()));

            // Assert
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.EqualTo("URL is required."));
        }

        [Test]
        public void IsValid_ShouldReturnError_ForInvalidUrl()
        {
            // Arrange
            var invalidUrl = "htp://invalid-url";

            // Act
            var result = _urlValidator.GetValidationResult(invalidUrl, new ValidationContext(new object()));

            // Assert
            Assert.That(result, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(result.ErrorMessage, Is.EqualTo("Invalid URL format."));
        }

        [Test]
        public void IsValid_ShouldReturnSuccess_ForValidUrlWithQueryString()
        {
            // Arrange
            var validUrlWithQueryString = "https://www.example.com?query=test";

            // Act
            var result = _urlValidator.GetValidationResult(validUrlWithQueryString, new ValidationContext(new object()));

            // Assert
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }
    }
}
