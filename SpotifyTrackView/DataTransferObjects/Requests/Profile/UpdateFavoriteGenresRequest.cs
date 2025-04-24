namespace SpotifyTrackView.DataTransferObjects.Requests.Profile;

public class UpdateFavoriteGenresRequest
{
    public List<int> MainGenreIds { get; set; } = new();
    public List<int> SubGenreIds { get; set; } = new();
    public List<int> OtherGenreIds { get; set; } = new();
}