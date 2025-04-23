namespace SpotifyTrackView.DataTransferObjects.Requests;

public class UpdateProfileDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
}
