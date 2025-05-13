using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using SpotifyTrackView.Enums;

namespace SpotifyTrackView.DataTransferObjects.Authentication;

public record RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; init; }

    [Required]
    public string Password { get; init; }
    
    [Required]
    [EnumDataType(typeof(UserRole))]
    public string Role { get; init; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; init; }
}