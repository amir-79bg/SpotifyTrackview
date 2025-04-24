using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Entity;

namespace SpotifyTrackView.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Listener> Listeners { get; set; }
    public DbSet<Influencer> Influencers { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Region> Regions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // 🎼 Genre self-referencing (subgenres)
        modelBuilder.Entity<Genre>()
            .HasOne(g => g.ParentGenre)
            .WithMany(g => g.SubGenres)
            .HasForeignKey(g => g.ParentGenreId)
            .OnDelete(DeleteBehavior.Restrict); // prevent cascade

        // 🎤 ArtistGenre: many-to-many with GenreType
        modelBuilder.Entity<ArtistGenre>()
            .HasKey(ag => new { ag.ArtistId, ag.GenreId });

        modelBuilder.Entity<ArtistGenre>()
            .HasOne(ag => ag.Artist)
            .WithMany(a => a.ArtistGenres)
            .HasForeignKey(ag => ag.ArtistId);

        modelBuilder.Entity<ArtistGenre>()
            .HasOne(ag => ag.Genre)
            .WithMany(g => g.ArtistGenres)
            .HasForeignKey(ag => ag.GenreId);

        modelBuilder.Entity<ArtistGenre>()
            .ToTable("ArtistGenres");

        // 🎧 ListenerGenre: simple many-to-many
        modelBuilder.Entity<ListenerGenre>()
            .HasKey(lg => new { lg.ListenerId, lg.GenreId });

        modelBuilder.Entity<ListenerGenre>()
            .HasOne(lg => lg.Listener)
            .WithMany(l => l.ListenerGenres)
            .HasForeignKey(lg => lg.ListenerId);

        modelBuilder.Entity<ListenerGenre>()
            .HasOne(lg => lg.Genre)
            .WithMany(g => g.ListenerGenres)
            .HasForeignKey(lg => lg.GenreId);

        modelBuilder.Entity<ListenerGenre>()
            .ToTable("ListenerGenres");
    }
}