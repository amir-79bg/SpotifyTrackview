using SpotifyTrackView.Entity;

namespace SpotifyTrackView.Resources;

public class UserResource
{
    public static object From(AppUser user, HttpRequest request)
    {
        string baseUrl = $"{request.Scheme}://{request.Host}";
        return new
        {
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            ThumbnailUrl = user.ThumbnailUrl != null ? baseUrl + user.ThumbnailUrl : null,
            user.Country,
            user.Region,
            user.CreatedAt,
            user.UpdatedAt
        };
    }
}