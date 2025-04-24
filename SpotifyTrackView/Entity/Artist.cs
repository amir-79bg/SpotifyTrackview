using System.ComponentModel.DataAnnotations;

namespace SpotifyTrackView.Entity;

public class Artist: AppUser
{
    public ICollection<ArtistGenre> ArtistGenres { get; set; } = new List<ArtistGenre>();
}