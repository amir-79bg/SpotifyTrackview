using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Controllers.Api.V1.Shared;
using SpotifyTrackView.Data;
using SpotifyTrackView.Entity;
using SpotifyTrackView.Enums;
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

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var playlists = await query
            .OrderByDescending(i => i.CreatedAt)
            .ThenByDescending(p => p.Status == PlaylistStatus.Processing)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => PlaylistResource.From(p, HttpContext.Request))
            .ToListAsync();

        return Ok(new
        {
            Data = playlists,
            TotalCount = totalCount,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = totalPages
        });
    }

    [HttpPut("{playlistId}")]
    public async Task<OkObjectResult> AcceptPlaylist(int playlistId)
    {
        Playlist playlist = (await context.Playlists.FindAsync(playlistId))!;

        playlist.Status = PlaylistStatus.Accepted;
        playlist.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync();

        return Ok(playlist);
    }
}