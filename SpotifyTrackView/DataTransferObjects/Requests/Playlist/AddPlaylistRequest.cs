using System.ComponentModel.DataAnnotations;

namespace SpotifyTrackView.DataTransferObjects.Requests.Playlist;

public class AddPlaylistRequest
{
    [Required] public required string PlaylistUrl { get; set; }
} 