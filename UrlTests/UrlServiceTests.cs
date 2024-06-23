namespace UrlTests
{
    using Moq;
    using NUnit.Framework;
    using Microsoft.Extensions.Logging;
    using UrlShortener.Models;
    using UrlShortener.Repositories;
    using UrlShortener.Services;

    public class UrlServiceTests
    {
        private Mock<IUrlRepository> _mockUrlRepository;
        private Mock<ILogger<UrlService>> _mockLogger;
        private IUrlService _urlService;

        [SetUp]
        public void Setup()
        {
            _mockUrlRepository = new Mock<IUrlRepository>();
            _mockLogger = new Mock<ILogger<UrlService>>();
            _urlService = new UrlService(_mockUrlRepository.Object, _mockLogger.Object);
        }

        [Test]
        public async Task ShortenUrlAsync_ShouldReturnShortenedUrl()
        {
            // Arrange
            var originalUrl = "https://www.example.com";
            _mockUrlRepository.Setup(repo => repo.GetShortenedUrlByShortenedUrlAsync(It.IsAny<string>())).ReturnsAsync((Url)null);
            _mockUrlRepository.Setup(repo => repo.Add(It.IsAny<Url>())).Returns(Task.CompletedTask);

            // Act
            var result = await _urlService.ShortenUrlAsync(originalUrl);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(8));
        }

        [Test]
        public async Task GetOriginalUrlFromShortenedAsync_ShouldReturnOriginalUrl()
        {
            // Arrange
            var shortenedUrl = "abcd1234";
            var originalUrl = "https://www.example.com";
            var url = new Url { OriginalUrl = originalUrl, ShortenedUrl = shortenedUrl };
            _mockUrlRepository.Setup(repo => repo.GetShortenedUrlByShortenedUrlAsync(shortenedUrl)).ReturnsAsync(url);

            // Act
            var result = await _urlService.GetOriginalUrlFromShortenedAsync(shortenedUrl);

            // Assert
            Assert.That(originalUrl, Is.EqualTo(result));
        }
    }
}
