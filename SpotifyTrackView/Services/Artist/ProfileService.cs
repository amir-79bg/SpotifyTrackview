using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Requests.Profile.Artist;
using SpotifyTrackView.Interfaces.Services.Artist;

namespace SpotifyTrackView.Services.Artist;

public class ProfileService(ApplicationDbContext context, IDistributedCache cache): IProfileService
{
    ApplicationDbContext _context = context;

    public async Task<Entity.Artist> UpdateSocialMedias(int artistId, UpdateSocialMediasRequest request)
    {
        Entity.Artist artist = (await _context.Artists.FindAsync(artistId))!;

        artist.SpotifyUrl = request.SpotifyUrl ?? artist.SpotifyUrl;
        artist.SoundcloudUrl = request.SoundcloudUrl ?? artist.SoundcloudUrl;
        artist.YoutubeUrl = request.YoutubeUrl ?? artist.YoutubeUrl;
        artist.InstagramUrl = request.InstagramUrl ?? artist.InstagramUrl;
        artist.FacebookUrl = request.FacebookUrl ?? artist.FacebookUrl;
        artist.TiktokUrl = request.TiktokUrl ?? artist.TiktokUrl;
        artist.WebsiteUrl = request.WebsiteUrl ?? artist.WebsiteUrl;

        _context.Artists.Update(artist);

        await _context.SaveChangesAsync();

        return artist;
    }
}