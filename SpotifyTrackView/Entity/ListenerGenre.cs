namespace SpotifyTrackView.Entity;

public class ListenerGenre
{
    public int ListenerId { get; set; }
    public Listener Listener { get; set; }
    
    public int GenreId { get; set; }
    public Genre Genre { get; set; }
}