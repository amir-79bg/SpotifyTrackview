namespace SpotifyTrackView.Interfaces;

public interface IAppUser
{
    int Id { get; set; }
    string Email { get; set; }
    string Password { get; set; }
}
