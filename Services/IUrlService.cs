namespace UrlShortener.Services
{
    public interface IUrlService
    {
        Task<string> ShortenUrlAsync(string originalUrl);
        Task<string> GetOriginalUrlAsync(string shortenedUrl);
    }
}
