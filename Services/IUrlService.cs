namespace UrlShortener.Services
{
    public interface IUrlService
    {
        Task<string> ShortenUrlAsync(string originalUrl);
        Task<string> GetOriginalUrlFromShortenedAsync(string shortenedUrl);
    }
}
