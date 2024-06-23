namespace UrlShortener.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using UrlShortener.Data;
    using UrlShortener.Models;

    public class UrlRepository : IUrlRepository
    {
        private readonly AppDbContext _context;

        public UrlRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Url> GetOriginalUrlByOriginalUrlAsync(string originalUrl)
        {
            return await _context.Urls.FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl);
        }

        public async Task<Url> GetShortenedUrlByShortenedUrlAsync(string shortenedUrl)
        {
            return await _context.Urls.FirstOrDefaultAsync(u => u.ShortenedUrl == shortenedUrl);
        }

        public async Task<bool> CheckShortenedUrlExistsAsync(string shortenedUrl)
        {
            return await _context.Urls.AnyAsync(u => u.ShortenedUrl == shortenedUrl);
        }

        public async Task Add(Url url)
        {
            _context.Urls.Add(url);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
