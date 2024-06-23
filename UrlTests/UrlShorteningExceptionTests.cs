namespace UrlTests
{
    using UrlShortener.Services;
    using NUnit.Framework;
    public class UrlShorteningExceptionTests
    {
        [Test]
        public void Constructor_WithMessageAndInvalidUrl_ShouldSetProperties()
        {
            // Arrange
            var message = "Invalid URL format.";
            var invalidUrl = "htp://invalid-url";

            // Act
            var exception = new UrlShorteningException(message, invalidUrl);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InvalidUrl, Is.EqualTo(invalidUrl));
        }

        [Test]
        public void Constructor_WithMessageInvalidUrlAndInnerException_ShouldSetProperties()
        {
            // Arrange
            var message = "Invalid URL format.";
            var invalidUrl = "htp://invalid-url";
            var innerException = new Exception("Inner exception message");

            // Act
            var exception = new UrlShorteningException(message, invalidUrl, innerException);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InvalidUrl, Is.EqualTo(invalidUrl));
            Assert.That(exception.InnerException, Is.EqualTo(innerException));
        }

        [Test]
        public void Constructor_WithMessageAndInnerException_ShouldSetProperties()
        {
            // Arrange
            var message = "Invalid URL format.";
            var innerException = new Exception("Inner exception message");

            // Act
            var exception = new UrlShorteningException(message, innerException);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InnerException, Is.EqualTo(innerException));
        }

        [Test]
        public void ToString_ShouldReturnCorrectStringRepresentation()
        {
            // Arrange
            var message = "Invalid URL format.";
            var invalidUrl = "htp://invalid-url";
            var exception = new UrlShorteningException(message, invalidUrl);

            // Act
            var result = exception.ToString();

            // Assert
            Assert.That(result, Does.Contain(message));
            Assert.That(result, Does.Contain(invalidUrl));
        }

        [Test]
        public void ToString_WithoutInvalidUrl_ShouldReturnBaseStringRepresentation()
        {
            // Arrange
            var message = "Invalid URL format.";
            var innerException = new Exception("Inner exception message");
            var exception = new UrlShorteningException(message, innerException);

            // Act
            var result = exception.ToString();

            // Assert
            Assert.That(result, Does.Contain(message));
            Assert.That(result, Does.Not.Contain("Invalid URL:"));
        }
    }
}
