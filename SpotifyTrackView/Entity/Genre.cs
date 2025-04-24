namespace SpotifyTrackView.Entity;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public int? ParentGenreId { get; set; }
    public Genre? ParentGenre { get; set; }
    public ICollection<Genre> SubGenres { get; set; } = new List<Genre>();
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    
    // Relations
    public ICollection<ArtistGenre> ArtistGenres { get; set; } = new List<ArtistGenre>();
    public ICollection<ListenerGenre> ListenerGenres { get; set; } = new List<ListenerGenre>();
}