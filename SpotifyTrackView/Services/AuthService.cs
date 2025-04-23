using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SpotifyTrackView.Interfaces;

namespace SpotifyTrackView.Services;

public class AuthService<TUser> : IAuthService<TUser> where TUser : class
{
    private readonly IPasswordHasher<TUser> _hasher;
    private readonly IConfiguration _configuration;

    public AuthService(IPasswordHasher<TUser> hasher, IConfiguration configuration)
    {
        _hasher = hasher;
        _configuration = configuration;
    }

    public string HashPassword(TUser user, string password)
    {
        return _hasher.HashPassword(user, password);
    }

    public bool VerifyPassword(TUser user, string hashedPassword, string inputPassword)
    {
        var result = _hasher.VerifyHashedPassword(user, hashedPassword, inputPassword);

        if (result != PasswordVerificationResult.Success)
        {
            return false;
        }

        return true;
    }

    public string GenerateJwtToken(TUser user, List<Claim> claims)
    {
        var userType = typeof(TUser).Name; // e.g., "Admin", "Artist", etc.
        var section = _configuration.GetSection($"JwtSettings:{userType}");

        if (!section.Exists())
            throw new InvalidOperationException($"JwtSettings for {userType} not found.");

        var issuer = section["Issuer"];
        var audience = section["Audience"];
        var secretKey = section["SecretKey"];

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException($"SecretKey is missing for JwtSettings:{userType}");

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}