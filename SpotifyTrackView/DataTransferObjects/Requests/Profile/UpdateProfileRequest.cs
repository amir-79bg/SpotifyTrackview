using System.ComponentModel.DataAnnotations;
using SpotifyTrackView.Entity;

namespace SpotifyTrackView.DataTransferObjects.Requests.Profile;

public class UpdateProfileRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public IFormFile? ThumbnailUrl { get; set; }

    [StringLength(2)]
    public string? Country { get; set; }

    [StringLength(2)]
    public string? Region { get; set; }

    public AppUser TransformToAppUser(int userId, string? thumbnailUrl)
    {
        AppUser user = new AppUser();
        user.Id = userId;
        user.FirstName = FirstName;
        user.LastName = LastName;
        user.Country = Country;
        user.Region = Region;
        user.ThumbnailUrl = thumbnailUrl;
        user.UpdatedAt = DateTime.Now;

        return user;
    }
}