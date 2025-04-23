using Microsoft.AspNetCore.Mvc;
using SpotifyTrackView.Controllers.Api.V1.Shared;
using SpotifyTrackView.Data;
using SpotifyTrackView.Interfaces;

namespace SpotifyTrackView.Controllers.Api.V1.Artist;


[ApiController]
[Route("api/v1/artists/authentication")]
public class AuthenticationController(ApplicationDbContext context, IAuthService<Entity.Artist> authService)
    : BaseAuthenticationController<Entity.Artist>(context, authService);