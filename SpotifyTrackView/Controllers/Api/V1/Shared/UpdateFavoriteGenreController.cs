using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Requests.Profile;
using SpotifyTrackView.Entity;
using SpotifyTrackView.Enums;

namespace SpotifyTrackView.Controllers.Api.V1.Shared;

[ApiController]
[Route("/api/v1/profile/favorite-genres")]
public class UpdateFavoriteGenreController: BaseApiController
{
    private ApplicationDbContext _db;

    public UpdateFavoriteGenreController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    [HttpPut]
    [Authorize(AuthenticationSchemes = "ListenerScheme,ArtistScheme")]
    public async Task<IActionResult> UpdateFavoriteGenres([FromBody] UpdateFavoriteGenresRequest request)
    {
        var userType = User.FindFirstValue(ClaimTypes.Role);
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        switch (userType)
        {
            case nameof(Entity.Artist):
                if (request.MainGenreIds.Count > 3 || request.SubGenreIds.Count > 15 || request.OtherGenreIds.Count > 3)
                    return BadRequest("Artist genre limits exceeded.");

                var artist = await _db.Artists
                    .Include(a => a.ArtistGenres)
                    .FirstOrDefaultAsync(a => a.Id == userId);

                artist.ArtistGenres.Clear();

                var allGenres = new List<ArtistGenre>();

                allGenres.AddRange(request.MainGenreIds.Select(id => new ArtistGenre { GenreId = id, Type = GenreType.Main }));
                allGenres.AddRange(request.SubGenreIds.Select(id => new ArtistGenre { GenreId = id, Type = GenreType.Sub }));
                allGenres.AddRange(request.OtherGenreIds.Select(id => new ArtistGenre { GenreId = id, Type = GenreType.Other }));

                foreach (var ag in allGenres)
                {
                    artist.ArtistGenres.Add(ag);
                }                
                
                await _db.SaveChangesAsync();
                return Ok("Genres updated for artist.");

            case nameof(Listener):
                if (request.MainGenreIds.Count > 5 || request.SubGenreIds.Any() || request.OtherGenreIds.Any())
                    return BadRequest("Listeners can only select up to 5 main genres.");

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
                return Ok(listener);

            default:
                return Unauthorized("Invalid user type.");
        }
    }

}