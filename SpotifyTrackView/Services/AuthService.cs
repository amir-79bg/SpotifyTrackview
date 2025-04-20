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
        var tokenHandler = new JwtSecurityTokenHandler();

        // Create a symmetric security key using the secret key from the configuration.
        var authSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(_configuration["JwtSettings:Admin:SecretKey"]));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["JwtSettings:Admin:Issuer"],
            Audience = _configuration["JwtSettings:Admin:Audience"],
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(15),
            SigningCredentials = new SigningCredentials
                (authSigningKey, SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}