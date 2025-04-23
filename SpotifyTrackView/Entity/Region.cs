namespace SpotifyTrackView.Entity;

public class Region
{
    public int Id {get; set;}
    public string Iso2 {get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}