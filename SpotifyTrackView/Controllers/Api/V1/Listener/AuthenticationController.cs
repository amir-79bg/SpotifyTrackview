using Microsoft.AspNetCore.Mvc;
using SpotifyTrackView.Controllers.Api.V1.Shared;
using SpotifyTrackView.Data;
using SpotifyTrackView.Interfaces;

namespace SpotifyTrackView.Controllers.Api.V1.Listener;

[ApiController]
[Route("api/v1/listeners/authentication")]
public class AuthenticationController(ApplicationDbContext context, IAuthService<Entity.Listener> authService)
    : BaseAuthenticationController<Entity.Listener>(context, authService);