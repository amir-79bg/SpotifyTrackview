using SpotifyTrackView.DataTransferObjects.Requests.Profile.Artist;

namespace SpotifyTrackView.Interfaces.Services.Artist;

public interface IProfileService
{
    Task<Entity.Artist> UpdateSocialMedias(int artistId, UpdateSocialMediasRequest request);
}