using System.ComponentModel.DataAnnotations;

namespace SpotifyTrackView.DataTransferObjects.Requests.Playlist;

public class AddPlaylistRequest
{
    [Required] public string PlaylistUrl { get; set; }
} 