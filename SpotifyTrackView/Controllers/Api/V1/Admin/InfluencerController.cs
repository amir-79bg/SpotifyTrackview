using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Controllers.Api.V1.Shared;
using SpotifyTrackView.Data;

namespace SpotifyTrackView.Controllers.Api.V1.Admin;

[ApiController]
[Route("/api/v1/admin/influencers")]
[Authorize(AuthenticationSchemes = "AdminScheme")]
public class InfluencerController(ApplicationDbContext context) : BaseApiController
{

    [HttpGet]
    public async Task<OkObjectResult> ListOfInfluencers()
    {
        var influencers = await context.Influencers.OrderByDescending(i => i.CreatedAt).ToListAsync();

        return Ok(influencers);
    }
}