using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;


namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly ILogger<UrlService> _logger;

        private const int retryLimit = 2;

        public UrlService(IUrlRepository urlRepository, ILogger<UrlService> logger)
        {
            _urlRepository = urlRepository;
            _logger = logger;
        }

        public async Task<string> ShortenUrlAsync(string originalUrl)
        {
            var existingUrl = await GetExistingShortenedUrlAsync(originalUrl);
            if (existingUrl != null)
            {
                return existingUrl;
            }

            var shortenedUrl = await GenerateUniqueShortUrlAsync();

            return await SaveUrlAsync(originalUrl, shortenedUrl);
        }

        public async Task<string> GetOriginalUrlAsync(string shortenedUrl)
        {
            var url = await _urlRepository.GetByShortenedUrlAsync(shortenedUrl);
            return url?.OriginalUrl;
        }

        private async Task<string> GetExistingShortenedUrlAsync(string originalUrl)
        {
            var urlEntry = await _urlRepository.GetByOriginalUrlAsync(originalUrl);
            return urlEntry?.ShortenedUrl;
        }

        private async Task<string> GenerateUniqueShortUrlAsync()
        {
            string shortenedUrl;
            do
            {
                shortenedUrl = GenerateShortUrl();
            } while (await _urlRepository.ExistsByShortenedUrlAsync(shortenedUrl));

            return shortenedUrl;
        }

        private string GenerateShortUrl()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        private async Task<string> SaveUrlAsync(string originalUrl, string shortenedUrl)
        {
            var url = new Url
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = shortenedUrl
            };

            _urlRepository.Add(url);

            for (int attempt = 0; attempt < retryLimit; attempt++)
            {
                if (await TrySaveChangesAsync(originalUrl, attempt))
                {
                    return shortenedUrl;
                }
                await Task.Delay(TimeSpan.FromSeconds(2));
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
                if (attempt == retryLimit - 1)
                {
                    _logger.LogError("Max retry attempts reached for URL '{OriginalUrl}'. Unable to save URL to database.", originalUrl);
                }
                return false;
            }
        }
    }
}
