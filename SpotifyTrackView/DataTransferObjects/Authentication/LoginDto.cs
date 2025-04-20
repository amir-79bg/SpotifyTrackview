using System.ComponentModel.DataAnnotations;

namespace SpotifyTrackView.DataTransferObjects.Authentication;

public record LoginDto(
    [Required] [EmailAddress] string Email,
    [Required] string Password
)
{
}