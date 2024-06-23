namespace UrlShortener.Services
{
    using Microsoft.EntityFrameworkCore;
    using UrlShortener.Models;
    using UrlShortener.Repositories;

    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly ILogger<UrlService> _logger;

        private const int RetryLimit = 2;
        private static readonly TimeSpan InitialDelay = TimeSpan.FromSeconds(2);

        public UrlService(IUrlRepository urlRepository, ILogger<UrlService> logger)
        {
            _urlRepository = urlRepository;
            _logger = logger;
        }

        public async Task<string> ShortenUrlAsync(string originalUrl)
        {
            // Check if the original URL has already been shortened
            var existingShortUrl = await CheckIfUrlHasBeenShortened(originalUrl);
            if (existingShortUrl != null)
            {
                // Return the pre-existing shortened URL, don't generate a new one
                return existingShortUrl;
            }

            var shortenedUrl = await GenerateUniqueShortUrlAsync();
            return await SaveUrlWithRetriesAsync(originalUrl, shortenedUrl);
        }

        public async Task<string> GetOriginalUrlFromShortenedAsync(string shortenedUrl)
        {
            //Searches DB for a short URL, checks if it exists, if it does, returns the original url that matches the short url
            var url = await _urlRepository.GetShortenedUrlByShortenedUrlAsync(shortenedUrl);
            return url?.OriginalUrl;
        }

        private async Task<string> CheckIfUrlHasBeenShortened(string originalUrl)
        {
            //Searches DB for original URL, checks if a short version exists, if it does, returns the short version
            var urlEntry = await _urlRepository.GetOriginalUrlByOriginalUrlAsync(originalUrl);
            return urlEntry?.ShortenedUrl;
        }

        private async Task<string> GenerateUniqueShortUrlAsync()
        {
            string shortenedUrl;
            do
            {
                shortenedUrl = Guid.NewGuid().ToString("N").Substring(0, 8);
            } while (await _urlRepository.CheckShortenedUrlExistsAsync(shortenedUrl));

            return shortenedUrl;
        }

        private async Task<string> SaveUrlWithRetriesAsync(string originalUrl, string shortenedUrl)
        {
            var url = new Url
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = shortenedUrl
            };

            _urlRepository.Add(url);

            for (int attempt = 0; attempt < RetryLimit; attempt++)
            {
                if (await TrySaveChangesAsync(originalUrl, attempt))
                {
                    return shortenedUrl;
                }
                await Task.Delay(ExponentialBackoff(attempt));
            }

            _logger.LogError("Error saving URL '{OriginalUrl}' to database after multiple attempts.", originalUrl);
            return null;
        }

        private async Task<bool> TrySaveChangesAsync(string originalUrl, int attempt)
        {
            try
            {
                await _urlRepository.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error saving URL '{OriginalUrl}' to database. Attempt {Attempt}. Error: {ErrorMessage}", originalUrl, attempt + 1, ex.Message);
                if (attempt == RetryLimit - 1)
                {
                    _logger.LogError("Max retry attempts reached for URL '{OriginalUrl}'. Unable to save URL to database.", originalUrl);
                }
                return false;
            }
        }

        private static TimeSpan ExponentialBackoff(int attempt)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attempt)) + InitialDelay;
        }
    }
}
