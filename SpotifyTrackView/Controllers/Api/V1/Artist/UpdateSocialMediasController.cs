using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotifyTrackView.DataTransferObjects.Requests.Profile.Artist;
using SpotifyTrackView.Interfaces.Services.Artist;

namespace SpotifyTrackView.Controllers.Api.V1.Artist;

[ApiController]
[Route("/api/v1/artists/social-medias")]
public class UpdateSocialMediasController(IProfileService profileService): ControllerBase
{
    [HttpPut]
    [Authorize(AuthenticationSchemes = "ArtistScheme")]
    public async Task<IActionResult> Update([FromBody] UpdateSocialMediasRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        Entity.Artist artist = await profileService.UpdateSocialMedias(userId, request);

        return Ok(artist);
    }

}