namespace UrlTests
{
    using Moq;
    using Microsoft.AspNetCore.Mvc;
    using UrlShortener.Controllers;
    using UrlShortener.Services;
    using NUnit.Framework;

    public class UrlControllerTests
    {
        private Mock<IUrlService> _mockUrlService;
        private UrlController _urlController;

        [SetUp]
        public void Setup()
        {
            _mockUrlService = new Mock<IUrlService>();
            _urlController = new UrlController(_mockUrlService.Object);
        }

        [Test]
        public async Task ShortenUrlAsync_ShouldReturnBadRequest_WhenOriginalUrlIsNullOrEmpty()
        {
            // Act
            var result = await _urlController.ShortenUrlAsync(null) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo("originalUrl is required."));
        }

        [Test]
        public async Task ShortenUrlAsync_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _urlController.ModelState.AddModelError("originalUrl", "Invalid URL");

            // Act
            var result = await _urlController.ShortenUrlAsync("invalid-url") as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task ShortenUrlAsync_ShouldReturnShortenedUrl_WhenValidUrl()
        {
            // Arrange
            var originalUrl = "https://www.example.com";
            var shortenedUrl = "abcd1234";
            _mockUrlService.Setup(service => service.ShortenUrlAsync(originalUrl)).ReturnsAsync(shortenedUrl);

            // Act
            var result = await _urlController.ShortenUrlAsync(originalUrl) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            // Using JObject to parse result.Value
            var response = Newtonsoft.Json.Linq.JObject.FromObject(result.Value);
            var shortenedUrlFromResponse = response.Value<string>("ShortenedUrl");

            Assert.That(shortenedUrlFromResponse, Is.EqualTo(shortenedUrl));
        }

        [Test]
        public async Task GetOriginalUrlAsync_ShouldReturnBadRequest_WhenFullUrlIsNullOrEmpty()
        {
            // Act
            var result = await _urlController.GetOriginalUrlAsync(null) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo("fullUrl is required."));
        }

        [Test]
        public async Task GetOriginalUrlAsync_ShouldReturnNotFound_WhenOriginalUrlIsNull()
        {
            // Arrange
            var fullUrl = "https://localhost:7256/Url/abcd1234";
            _mockUrlService.Setup(service => service.GetOriginalUrlFromShortenedAsync(It.IsAny<string>())).ReturnsAsync((string)null);

            // Act
            var result = await _urlController.GetOriginalUrlAsync(fullUrl) as NotFoundResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task GetOriginalUrlAsync_ShouldReturnOriginalUrl_WhenValidShortenedUrl()
        {
            // Arrange
            var fullUrl = "https://localhost:7256/Url/abcd1234";
            var originalUrl = "https://www.example.com";
            _mockUrlService.Setup(service => service.GetOriginalUrlFromShortenedAsync(It.IsAny<string>())).ReturnsAsync(originalUrl);

            // Act
            var result = await _urlController.GetOriginalUrlAsync(fullUrl) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));

            // Using JObject to parse result.Value
            var response = Newtonsoft.Json.Linq.JObject.FromObject(result.Value);
            var originalUrlFromResponse = response.Value<string>("OriginalUrl");

            Assert.That(originalUrlFromResponse, Is.EqualTo(originalUrl));
        }

        [Test]
        public async Task RedirectToOriginalUrlAsync_ShouldReturnNotFound_WhenOriginalUrlIsNull()
        {
            // Arrange
            var shortenedUrl = "abcd1234";
            _mockUrlService.Setup(service => service.GetOriginalUrlFromShortenedAsync(shortenedUrl)).ReturnsAsync((string)null);

            // Act
            var result = await _urlController.RedirectToOriginalUrlAsync(shortenedUrl) as NotFoundResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task RedirectToOriginalUrlAsync_ShouldRedirectToOriginalUrl_WhenValidShortenedUrl()
        {
            // Arrange
            var shortenedUrl = "abcd1234";
            var originalUrl = "https://www.example.com";
            _mockUrlService.Setup(service => service.GetOriginalUrlFromShortenedAsync(shortenedUrl)).ReturnsAsync(originalUrl);

            // Act
            var result = await _urlController.RedirectToOriginalUrlAsync(shortenedUrl) as RedirectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Url, Is.EqualTo(originalUrl));
        }
    }
}
