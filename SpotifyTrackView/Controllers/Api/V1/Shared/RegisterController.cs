using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Authentication;
using SpotifyTrackView.Interfaces;
using SpotifyTrackView.Resources.Shared;

namespace SpotifyTrackView.Controllers.Api.V1.Shared
{
    [Route("api/v1/register")]
    [ApiController]
    public class RegisterController(ApplicationDbContext context, IServiceProvider services) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            bool emailExists = await context.Influencers.AnyAsync(x => x.Email == request.Email)
                               || await context.Artists.AnyAsync(x => x.Email == request.Email)
                               || await context.Listeners.AnyAsync(x => x.Email == request.Email);
        
            if (emailExists)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "Email", new[] { "This email is already registered under another user type." } }
                };

                return ValidationProblem(new ValidationProblemDetails(errors));
            }

            switch (request.Role.ToLowerInvariant())
            {
                case "influencer":
                {
                    var user = new Entity.Influencer { Email = request.Email };
                    var auth = services.GetRequiredService<IAuthService<Entity.Influencer>>();
                    user.Password = auth.HashPassword(user, request.Password);

                    context.Influencers.Add(user);
                    await context.SaveChangesAsync();

                    var token = auth.GenerateJwtToken(user, BuildClaims(user, "Influencer"));
                    return Ok(AuthResource.From(user, token));
                }

                case "artist":
                {
                    var user = new Entity.Artist { Email = request.Email };
                    var auth = services.GetRequiredService<IAuthService<Entity.Artist>>();
                    user.Password = auth.HashPassword(user, request.Password);

                    context.Artists.Add(user);
                    await context.SaveChangesAsync();

                    var token = auth.GenerateJwtToken(user, BuildClaims(user, "Artist"));
                    return Ok(AuthResource.From(user, token));
                }

                case "listener":
                {
                    var user = new Entity.Listener { Email = request.Email };
                    var auth = services.GetRequiredService<IAuthService<Entity.Listener>>();
                    user.Password = auth.HashPassword(user, request.Password);

                    context.Listeners.Add(user);
                    await context.SaveChangesAsync();

                    var token = auth.GenerateJwtToken(user, BuildClaims(user, "Listener"));
                    return Ok(AuthResource.From(user, token));
                }

                default:
                    return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]>
                    {
                        { "Role", new[] { "Invalid role. Must be one of: Influencer, Artist, Listener." } }
                    }));
            }
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