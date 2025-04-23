using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Requests;
using SpotifyTrackView.Entity;

namespace SpotifyTrackView.Controllers.Api.V1.Shared;

[ApiController]
[Route("/api/v1/profile")]
public class ProfileController: ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProfileController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = "InfluencerScheme,ListenerScheme,ArtistScheme")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role);

        AppUser user = role switch
        {
            nameof(Entity.Artist) => await _context.Artists.FindAsync(userId),
            nameof(Entity.Influencer) => await _context.Influencers.FindAsync(userId),
            nameof(Entity.Listener) => await _context.Listeners.FindAsync(userId),
        };
        
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.ThumbnailUrl = dto.ThumbnailUrl;
        user.Country = dto.Country;
        user.Region = dto.Region;
        user.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return Ok(user);
    }
}