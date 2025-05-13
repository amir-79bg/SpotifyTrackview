using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Authentication;
using SpotifyTrackView.Interfaces;

namespace SpotifyTrackView.Controllers.Api.V1.Admin
{
    [Route("api/v1/admin/login")]
    [ApiController]
    public class LoginController(ApplicationDbContext context, IAuthService<Entity.Admin> authService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var admin = await context.Admins.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (admin != null)
            {
                if (!authService.VerifyPassword(admin, admin.Password, request.Password))
                    return Unauthorized(new ValidationProblemDetails(new Dictionary<string, string[]>
                    {
                        { "Password", new[] { "Wrong credentials." } }
                    }));
                var claims = BuildClaims(admin, "Admin");
                var token = authService.GenerateJwtToken(admin, claims);
                return Ok(new
                {
                    Token = token,
                    Admin = admin
                });
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