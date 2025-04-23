using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Authentication;
using SpotifyTrackView.Interfaces;

namespace SpotifyTrackView.Controllers.Api.V1.Shared;

[ApiController]
public abstract class BaseAuthenticationController<TUser> : ControllerBase where TUser : class, IAppUser, new()
{
    private readonly DbSet<TUser> _dbSet;
    private readonly IAuthService<TUser> _authService;
    private readonly ApplicationDbContext _context;

    protected BaseAuthenticationController(ApplicationDbContext context, IAuthService<TUser> authService)
    {
        _context = context;
        _authService = authService;
        _dbSet = context.Set<TUser>();
    }

    [HttpPost("login")]
    public virtual async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var appUser = await _dbSet.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (appUser == null || !_authService.VerifyPassword(appUser, appUser.Password, loginDto.Password))
        {
            return Unauthorized(new { Message = "Credential is wrong." });
        }

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
            new Claim(ClaimTypes.Email, appUser.Email),
            new Claim(ClaimTypes.Role, typeof(TUser).Name),
        };

        return Ok(new { Token = _authService.GenerateJwtToken(appUser, claims) });
    }

    [HttpPost("register")]
    public virtual async Task<TUser> Register([FromBody] RegisterDto registerDto)
    {
        TUser appUser = new TUser();
        appUser.Email = registerDto.Email;
        appUser.Password = _authService.HashPassword(appUser, registerDto.Password);

        _dbSet.Add(appUser);
        await _context.SaveChangesAsync();

        return appUser;
    }
}
