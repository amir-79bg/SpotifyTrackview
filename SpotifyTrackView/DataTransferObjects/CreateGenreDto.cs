using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SpotifyTrackView.DataTransferObjects;

public record CreateGenreDto
{
    [Required] [StringLength(100)] public string Name { get; init; }
    public int? ParentId { get; init; }
}