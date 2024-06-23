namespace UrlTests
{
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using UrlShortener.Data;
    using UrlShortener.Models;
    using UrlShortener.Repositories;

    public class UrlRepositoryTests
    {
        private DbContextOptions<AppDbContext> _dbContextOptions;
        private AppDbContext _context;
        private UrlRepository _urlRepository;

        [SetUp]
        public void Setup()
        {
            // Set up the in-memory database options
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UrlShortenerDb")
                .Options;

            _context = new AppDbContext(_dbContextOptions);
            _urlRepository = new UrlRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetOriginalUrlByOriginalUrlAsync_ShouldReturnUrl()
        {
            // Arrange
            var originalUrl = "https://www.example.com";
            var url = new Url { OriginalUrl = originalUrl, ShortenedUrl = "abcd1234" };
            await _urlRepository.Add(url);
            await _urlRepository.SaveChangesAsync();

            // Act
            var result = await _urlRepository.GetOriginalUrlByOriginalUrlAsync(originalUrl);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.OriginalUrl, Is.EqualTo(originalUrl));
        }

        [Test]
        public async Task GetShortenedUrlByShortenedUrlAsync_ShouldReturnUrl()
        {
            // Arrange
            var shortenedUrl = "abcd1234";
            var url = new Url { OriginalUrl = "https://www.example.com", ShortenedUrl = shortenedUrl };
            await _urlRepository.Add(url);
            await _urlRepository.SaveChangesAsync();

            // Act
            var result = await _urlRepository.GetShortenedUrlByShortenedUrlAsync(shortenedUrl);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ShortenedUrl, Is.EqualTo(shortenedUrl));
        }

        [Test]
        public async Task CheckShortenedUrlExistsAsync_ShouldReturnTrueIfExists()
        {
            // Arrange
            var shortenedUrl = "abcd1234";
            var url = new Url { OriginalUrl = "https://www.example.com", ShortenedUrl = shortenedUrl };
            await _urlRepository.Add(url);
            await _urlRepository.SaveChangesAsync();

            // Act
            var result = await _urlRepository.CheckShortenedUrlExistsAsync(shortenedUrl);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task CheckShortenedUrlExistsAsync_ShouldReturnFalseIfNotExists()
        {
            // Act
            var result = await _urlRepository.CheckShortenedUrlExistsAsync("nonexistent");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task Add_ShouldAddUrlToDatabase()
        {
            // Arrange
            var url = new Url { OriginalUrl = "https://www.example.com", ShortenedUrl = "abcd1234" };

            // Act
            await _urlRepository.Add(url);
            await _urlRepository.SaveChangesAsync();
            var result = await _context.Urls.FirstOrDefaultAsync(u => u.OriginalUrl == "https://www.example.com");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.OriginalUrl, Is.EqualTo("https://www.example.com"));
        }
    }
}
