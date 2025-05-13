using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Authentication;
using SpotifyTrackView.Interfaces;
using SpotifyTrackView.Resources.Shared;

namespace SpotifyTrackView.Controllers.Api.V1.Shared
{
    [Route("api/v1/login")]
    [ApiController]
    public class LoginController(ApplicationDbContext context, IServiceProvider services) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var listener = await context.Listeners.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (listener != null)
            {
                var authService = services.GetRequiredService<IAuthService<Entity.Listener>>();
                if (!authService.VerifyPassword(listener, listener.Password, request.Password))
                    return Unauthorized(new ValidationProblemDetails(new Dictionary<string, string[]>
                    {
                        { "Password", new[] { "Wrong credentials." } }
                    }));
                var claims = BuildClaims(listener, "Listener");
                var token = authService.GenerateJwtToken(listener, claims);
                return Ok(AuthResource.From(listener, token));
            }

            var artist = await context.Artists.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (artist != null)
            {
                var authService = services.GetRequiredService<IAuthService<Entity.Artist>>();
                if (!authService.VerifyPassword(artist, artist.Password, request.Password))
                    return Unauthorized(new ValidationProblemDetails(new Dictionary<string, string[]>
                    {
                        { "Password", new[] { "Wrong credentials." } }
                    }));
                var claims = BuildClaims(artist, "Artist");
                var token = authService.GenerateJwtToken(artist, claims);
                return Ok(AuthResource.From(artist, token));
            }

            var influencer = await context.Influencers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (influencer != null)
            {
                var authService = services.GetRequiredService<IAuthService<Entity.Influencer>>();
                if (!authService.VerifyPassword(influencer, influencer.Password, request.Password))
                    return Unauthorized(new ValidationProblemDetails(new Dictionary<string, string[]>
                    {
                        { "Password", new[] { "Wrong credentials." } }
                    }));
                var claims = BuildClaims(influencer, "Influencer");
                var token = authService.GenerateJwtToken(influencer, claims);
                return Ok(AuthResource.From(influencer, token));
            }

            return Unauthorized(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Email", new[] { "No account exists with this email." } }
            }));
        }

        private List<Claim> BuildClaims(dynamic user, string role) =>
            new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };
    }
}