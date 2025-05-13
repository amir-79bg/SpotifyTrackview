using SpotifyTrackView.Entity;

namespace SpotifyTrackView.Resources.Shared;

public class AuthResource
{
    public static object From(AppUser user, string token)
    {
        return new
        {
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            Token = token,
            user.Bio,
            user.Country,
            user.Region,
            user.CreatedAt,
            user.UpdatedAt
        };
    }
}