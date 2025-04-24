using System.ComponentModel.DataAnnotations;

namespace SpotifyTrackView.Entity;

public class Influencer: AppUser
{
    public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}