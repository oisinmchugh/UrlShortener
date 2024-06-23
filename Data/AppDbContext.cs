namespace UrlShortener.Data
{
    using Microsoft.EntityFrameworkCore;
    using UrlShortener.Models;

    public class AppDbContext : DbContext
    {
        public DbSet<Url> Urls { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Url>()
                .HasIndex(u => u.ShortenedUrl)
                .IsUnique();
        }
    }
}
