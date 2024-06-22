using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly AppDbContext _context;
        public UrlService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> ShortenUrlAsync(string originalUrl)
        {
            // Check for duplicate URL
            var existingUrl = await _context.Urls.FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl);
            if (existingUrl != null)
            {
                return existingUrl.ShortenedUrl;
            }

            // Generate shortened URL
            var shortenedUrl = GenerateShortUrl();

            var url = new Url
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = shortenedUrl
            };

            _context.Urls.Add(url);
            await _context.SaveChangesAsync();

            return shortenedUrl;
        }

        public async Task<string> GetOriginalUrlAsync(string shortenedUrl)
        {
            var url = await _context.Urls.FirstOrDefaultAsync(u => u.ShortenedUrl == shortenedUrl);
            return url?.OriginalUrl;
        }

        private string GenerateShortUrl()
        {
            return Guid.NewGuid().ToString().Substring(0, 6);
        }
    }
}
