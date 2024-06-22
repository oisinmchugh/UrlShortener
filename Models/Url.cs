using System.ComponentModel.DataAnnotations;

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
