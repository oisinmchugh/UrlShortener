using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly AppDbContext _context;

        public UrlRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Url> GetByOriginalUrlAsync(string originalUrl)
        {
            return await _context.Urls.FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl);
        }

        public async Task<Url> GetByShortenedUrlAsync(string shortenedUrl)
        {
            return await _context.Urls.FirstOrDefaultAsync(u => u.ShortenedUrl == shortenedUrl);
        }

        public async Task<bool> ExistsByShortenedUrlAsync(string shortenedUrl)
        {
            return await _context.Urls.AnyAsync(u => u.ShortenedUrl == shortenedUrl);
        }

        public void Add(Url url)
        {
            _context.Urls.Add(url);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
