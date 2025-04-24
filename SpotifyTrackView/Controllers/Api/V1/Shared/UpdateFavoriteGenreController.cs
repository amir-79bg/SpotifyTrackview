using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Requests.Profile;
using SpotifyTrackView.Entity;
using SpotifyTrackView.Enums;
using SpotifyTrackView.Validation.Rules;

namespace SpotifyTrackView.Controllers.Api.V1.Shared;

[ApiController]
[Route("/api/v1/profile/favorite-genres")]
public class UpdateFavoriteGenreController : BaseApiController
{
    private ApplicationDbContext _db;

    public UpdateFavoriteGenreController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = "ListenerScheme,ArtistScheme")]
    public async Task<IActionResult> UpdateFavoriteGenres(
        [FromBody] UpdateFavoriteGenresRequest request,
        [FromServices] IServiceProvider services
    )
    {
        var userType = User.FindFirstValue(ClaimTypes.Role);
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        switch (userType)
        {
            case nameof(Entity.Artist):
                var artistValidator = services.GetRequiredService<ArtistFavoriteGenreValidator>();
                var result = await artistValidator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    return ValidationError(result.Errors);
                }

                var artist = await _db.Artists
                    .Include(a => a.ArtistGenres)
                    .FirstOrDefaultAsync(a => a.Id == userId);

                artist.ArtistGenres.Clear();

                var allGenres = new List<ArtistGenre>();

                allGenres.AddRange(request.MainGenreIds.Select(id => new ArtistGenre
                    { GenreId = id, Type = GenreType.Main }));
                allGenres.AddRange(request.SubGenreIds.Select(id => new ArtistGenre
                    { GenreId = id, Type = GenreType.Sub }));
                allGenres.AddRange(request.OtherGenreIds.Select(id => new ArtistGenre
                    { GenreId = id, Type = GenreType.Other }));

                foreach (var ag in allGenres)
                {
                    artist.ArtistGenres.Add(ag);
                }

                await _db.SaveChangesAsync();
                return Success();

            case nameof(Listener):
                var listenerValidator = services.GetRequiredService<ListenerFavoriteGenreValidator>();
                var listenerResultValidator = await listenerValidator.ValidateAsync(request);
                if (!listenerResultValidator.IsValid)
                {
                    return ValidationError(listenerResultValidator.Errors);
                }

                var listener = await _db.Listeners
                    .Include(l => l.ListenerGenres)
                    .FirstOrDefaultAsync(l => l.Id == userId);

                listener.ListenerGenres.Clear();

                var listenerAllGenres = new List<ListenerGenre>();

                listenerAllGenres.AddRange(request.MainGenreIds.Select(id => new ListenerGenre { GenreId = id }));

                foreach (var ag in listenerAllGenres)
                {
                    listener.ListenerGenres.Add(ag);
                }

                await _db.SaveChangesAsync();
                return Success();

            default:
                return Unauthorized("Invalid user type.");
        }
    }
}