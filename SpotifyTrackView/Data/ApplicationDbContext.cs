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
}