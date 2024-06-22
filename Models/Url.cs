using System.ComponentModel.DataAnnotations;
using UrlShortener.Validation;

namespace UrlShortener.Models
{
    public class Url
    {
        public int Id { get; set; }

        [Required]
        public string OriginalUrl { get; set; }
        public string ShortenedUrl { get; set; }
    }
}
