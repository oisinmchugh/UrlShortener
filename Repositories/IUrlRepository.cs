﻿namespace UrlShortener.Repositories
{
    using UrlShortener.Models;

    public interface IUrlRepository
    {
        Task<Url> GetOriginalUrlByOriginalUrlAsync(string originalUrl);
        Task<Url> GetShortenedUrlByShortenedUrlAsync(string shortenedUrl);
        Task<bool> CheckShortenedUrlExistsAsync(string shortenedUrl);
        Task Add(Url url);
        Task SaveChangesAsync();
    }
}