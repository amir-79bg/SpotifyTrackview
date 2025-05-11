using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Requests.Playlist;
using SpotifyTrackView.DataTransferObjects.Requests.Profile;

namespace SpotifyTrackView.Validation.Rules;

public class AddPlaylistValidator : AbstractValidator<AddPlaylistRequest>
{
    public AddPlaylistValidator(ApplicationDbContext db)
    {
        RuleFor(x => x.PlaylistUrl)
            .MustAsync(async (input, _) =>
            {
                var spotifyId = input.Contains('/')
                    ? input.Split('/').LastOrDefault()?.Split('?').FirstOrDefault()
                    : input;

                return !await db.Playlists.AnyAsync(p => p.SpotifyId == spotifyId);
            })
            .WithMessage("This playlist has already been added.");
    }
}