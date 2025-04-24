using System.ComponentModel.DataAnnotations;
using SpotifyTrackView.Interfaces;

namespace SpotifyTrackView.Entity;

public class AppUser: IAppUser
{
    public int Id { get; set; }
    [StringLength(100)] public string Email { get; set; }
    [StringLength(100)] public string Password { get; set; }
    [StringLength(100)] public string? FirstName { get; set; }
    [StringLength(100)] public string? LastName { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? Bio {get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}