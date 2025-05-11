using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Controllers.Api.V1.Shared;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Requests.Playlist;
using SpotifyTrackView.Entity;
using SpotifyTrackView.Interfaces;

namespace SpotifyTrackView.Controllers.Api.V1.Influencer;

[ApiController]
[Authorize(AuthenticationSchemes = "InfluencerScheme")]
[Route("api/v1/influencer/playlists")]
public class AddPlaylistController(
    ApplicationDbContext context,
    ISpotifyPlaylistRepository spotifyPlaylistRepository)
    : BaseApiController
{
    private readonly ApplicationDbContext _context = context;
    private readonly ISpotifyPlaylistRepository _spotifyPlaylistRepository = spotifyPlaylistRepository;

    [HttpPost]
    public async Task<IActionResult> AddPlaylist(
        [FromBody] AddPlaylistRequest request,
        IValidator<AddPlaylistRequest> validator
    )
    {
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
        {
            return ValidationError(result.Errors);
        }

        var influencerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Extract playlist ID from Spotify URL
        var playlistId = request.PlaylistUrl.Split('/').Last().Split('?').FirstOrDefault();

        try
        {
            // Get Spotify access token
            var spotifyPlaylist = await _spotifyPlaylistRepository.GetPublicPlaylistAsync(playlistId);

            // Create new playlist entity
            var playlist = new Playlist
            {
                SpotifyId = playlistId,
                Name = spotifyPlaylist.Name,
                ThumbnailUrl = spotifyPlaylist.Images?.FirstOrDefault()?.Url,
                InfluencerId = int.Parse(influencerId),
                CreatedAt = DateTime.UtcNow
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            // Get all playlists for the influencer
            var influencerPlaylists = await _context.Playlists
                .Where(p => p.InfluencerId == int.Parse(influencerId))
                .Select(p => new
                {
                    p.Id,
                    p.SpotifyId,
                    p.Name,
                    p.ThumbnailUrl,
                    p.Status,
                    p.CreatedAt
                }).OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return Ok(influencerPlaylists);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Failed to add playlist", Error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPlaylists()
    {
        var influencerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(influencerId))
        {
            return Unauthorized();
        }

        var playlists = await _context.Playlists
            .Where(p => p.InfluencerId == int.Parse(influencerId))
            .Select(p => new
            {
                p.Id,
                p.SpotifyId,
                p.Name,
                p.ThumbnailUrl,
                p.Status,
                p.CreatedAt
            })
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return Ok(playlists);
    }
}