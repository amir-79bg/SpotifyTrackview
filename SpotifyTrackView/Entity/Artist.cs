using System.ComponentModel.DataAnnotations;

namespace SpotifyTrackView.Entity;

public class Artist: AppUser
{
    public string? SpotifyUrl { get; set; }
    public string? SoundcloudUrl { get; set; }
    public string? YoutubeUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? FacebookUrl { get; set; }
    public string? TiktokUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    
    public ICollection<ArtistGenre> ArtistGenres { get; set; } = new List<ArtistGenre>();
}