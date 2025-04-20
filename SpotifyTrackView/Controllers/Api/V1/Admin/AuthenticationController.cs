using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Authentication;
using SpotifyTrackView.Interfaces;

namespace SpotifyTrackView.Controllers.Api.V1.Admin;

[ApiController]
[Route("api/v1/admin/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService<Entity.Admin> _authService;
    private readonly ApplicationDbContext _context;

    // ReSharper disable once ConvertToPrimaryConstructor
    public AuthenticationController(ApplicationDbContext context, IAuthService<Entity.Admin> authService)
    {
        _context = context;
        _authService = authService;
    }

    // [Authorize(AuthenticationSchemes = "AdminScheme")]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var admin = await _context.Admins.FirstOrDefaultAsync(admin => admin.Email == loginDto.Email);
        if (admin == null)
        {
            return Unauthorized(new
            {
                Message = "Credential is wrong."
            });
        }

        var result = _authService.VerifyPassword(admin, admin.Password, loginDto.Password);
        if (result == false)
        {
            return Unauthorized(new
            {
                Message = "Credential is wrong."
            });
        }


        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
            new Claim(ClaimTypes.Email, admin.Email),
            new Claim(ClaimTypes.Role, nameof(admin)),
        };

        return Ok(new
        {
            Token = _authService.GenerateJwtToken(admin, claims),
        });
    }

    [HttpPost("register")]
    public async Task<Entity.Admin> Register([FromBody] RegisterDto registerDto)
    {
        Entity.Admin admin = new Entity.Admin();

        string hashedPassword = _authService.HashPassword(admin, registerDto.Password);
        admin.Email = registerDto.Email;
        admin.Password = hashedPassword;
        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();

        return admin;
    }
}