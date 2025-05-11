using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Controllers.Api.V1.Shared;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Authentication;
using SpotifyTrackView.Interfaces;

namespace SpotifyTrackView.Controllers.Api.V1.Admin;


[ApiController]
[Route("api/v1/admin/authentication")]
public class AuthenticationController(ApplicationDbContext context, IAuthService<Entity.Admin> authService)
    : BaseAuthenticationController<Entity.Admin>(context, authService);