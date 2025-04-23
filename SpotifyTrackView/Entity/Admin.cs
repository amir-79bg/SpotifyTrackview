using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Interfaces;

namespace SpotifyTrackView.Entity;

[Index(nameof(Password), IsUnique = true)]
public class Admin: IAppUser
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}