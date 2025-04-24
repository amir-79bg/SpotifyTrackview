using System.ComponentModel.DataAnnotations;

namespace SpotifyTrackView.Entity;

public class Listener: AppUser
{
    public ICollection<ListenerGenre> ListenerGenres { get; set; } = new List<ListenerGenre>();
}