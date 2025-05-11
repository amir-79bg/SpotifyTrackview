using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Controllers.Api.V1.Shared;
using SpotifyTrackView.Data;
using SpotifyTrackView.Entity;
using SpotifyTrackView.Enums;
using SpotifyTrackView.Helpers;
using SpotifyTrackView.Resources.Admin;

namespace SpotifyTrackView.Controllers.Api.V1.Admin;

[ApiController]
[Route("/api/v1/admin/playlists")]
[Authorize(AuthenticationSchemes = "AdminScheme")]
public class PlaylistController(ApplicationDbContext context) : BaseApiController
{

    [HttpGet]
    public async Task<OkObjectResult> List(
        [FromQuery] string? email = null,
        [FromQuery] string? name = null,
        [FromQuery] PlaylistStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        IQueryable<Playlist> query = context.Playlists
            .Include(p => p.Influencer);

        if (!string.IsNullOrEmpty(email))
        {
            query = query.Where(p => p.Influencer.Email.Contains(email));
        }

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(p => p.Name.Contains(name));
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        var result = await PaginationHelper.PaginateAsync(
            query.OrderByDescending(p => p.CreatedAt)
                .ThenByDescending(p => p.Status == PlaylistStatus.Processing),
            p => PlaylistResource.From(p, HttpContext.Request),
            page,
            pageSize
        );

        return Ok(result);
    }

    [HttpPut("{playlistId}")]
    public async Task<OkObjectResult> AcceptPlaylist(int playlistId)
    {
        Playlist playlist = (await context.Playlists.FindAsync(playlistId))!;

        playlist.Status = PlaylistStatus.Accepted;
        playlist.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync();

        return Ok(PlaylistResource.From(playlist, HttpContext.Request));
    }
}