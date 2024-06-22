using UrlShortener.Models;

public interface IUrlRepository
{
    Task<Url> GetByOriginalUrlAsync(string originalUrl);
    Task<Url> GetByShortenedUrlAsync(string shortenedUrl);
    Task<bool> ExistsByShortenedUrlAsync(string shortenedUrl);
    void Add(Url url);
    Task SaveChangesAsync();
}