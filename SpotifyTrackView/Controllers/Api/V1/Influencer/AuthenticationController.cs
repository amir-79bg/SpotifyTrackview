using Microsoft.AspNetCore.Mvc;
using SpotifyTrackView.Controllers.Api.V1.Shared;
using SpotifyTrackView.Data;
using SpotifyTrackView.Interfaces;

namespace SpotifyTrackView.Controllers.Api.V1.Influencer;

[ApiController]
[Route("api/v1/influencers/authentication")]
public class AuthenticationController(ApplicationDbContext context, IAuthService<Entity.Influencer> authService)
    : BaseAuthenticationController<Entity.Influencer>(context, authService);