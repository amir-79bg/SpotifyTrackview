using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Enums;

namespace SpotifyTrackView.Entity;


[Index(nameof(SpotifyId))]
public class Playlist
{
    public int Id { get; set; }
    
    public string SpotifyId { get; set; }
    public string Name { get; set; }
    public string ThumbnailUrl { get; set; }
    
    public int InfluencerId { get; set; }
    public Influencer Influencer { get; set; }

    public PlaylistStatus Status { get; set; } = PlaylistStatus.Processing;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}