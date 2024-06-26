﻿namespace UrlShortener.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using UrlShortener.Services;
    using UrlShortener.Validation;

    [ApiController]
    [Route("[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpPost]
        public async Task<IActionResult> ShortenUrlAsync([FromBody][UrlValidator] string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                return BadRequest($"{nameof(originalUrl)} is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shortenedUrl = await _urlService.ShortenUrlAsync(originalUrl);
            return Ok(new { ShortenedUrl = shortenedUrl });
        }

        // This is included as a potenial security feature to allow users to reverse check a shortened URL
        [HttpGet("originalUrl")]
        public async Task<IActionResult> GetOriginalUrlAsync(string fullUrl)
        {
            if (string.IsNullOrEmpty(fullUrl))
            {
                return BadRequest($"{nameof(fullUrl)} is required.");
            }

            // Getting the short url code from the entire URL
            var shortUrlCode = GetShortUrl(fullUrl);

            var originalUrl = await _urlService.GetOriginalUrlFromShortenedAsync(shortUrlCode);
            if (originalUrl == null)
            {
                return NotFound();
            }
            return Ok(new { OriginalUrl = originalUrl });
        }

        [HttpGet("r/{shortenedUrl}")]
        public async Task<IActionResult> RedirectToOriginalUrlAsync(string shortenedUrl)
        {
            var originalUrl = await _urlService.GetOriginalUrlFromShortenedAsync(shortenedUrl);
            if (originalUrl == null)
            {
                return NotFound();
            }
            return Redirect(originalUrl);
        }

        private string GetShortUrl(string fullUrl)
        {
            var urlLegnth = fullUrl.Length;
            var shortUrlCode = fullUrl.Substring(urlLegnth - 8, 8);
            return shortUrlCode;
        }
    }
}
