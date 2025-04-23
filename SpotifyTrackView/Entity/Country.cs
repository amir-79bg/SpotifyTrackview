namespace SpotifyTrackView.Entity;

public class Country
{
    public int Id {get; set;}
    public string Iso2 {get; set;}
    public string Name {get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}