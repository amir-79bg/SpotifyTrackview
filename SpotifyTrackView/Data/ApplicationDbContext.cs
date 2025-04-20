using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Entity;

namespace SpotifyTrackView.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Genre> Genres { get; set; }
}