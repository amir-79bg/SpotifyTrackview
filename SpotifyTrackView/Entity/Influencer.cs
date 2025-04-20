using System.ComponentModel.DataAnnotations;

namespace SpotifyTrackView.Entity;

public class Influencer
{
    public int Id { get; set; }
    [StringLength(100)] public string Name { get; set; }
    [StringLength(100)] public string Email { get; set; }
    [StringLength(100)] public string Password { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}