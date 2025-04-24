namespace SpotifyTrackView.Entity;

public class Playlist
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    public string ThumbnailUrl { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    
    public int InfluencerId { get; set; }
    public Influencer Influencer { get; set; }
}