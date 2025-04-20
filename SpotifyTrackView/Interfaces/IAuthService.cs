using System.Security.Claims;

namespace SpotifyTrackView.Interfaces;

public interface IAuthService<in TUser>
{
    string HashPassword(TUser user, string password);
    bool VerifyPassword(TUser user, string hashedPassword, string inputPassword);
    string GenerateJwtToken(TUser user, List<Claim> claims);
}
