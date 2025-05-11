using Microsoft.OpenApi.Extensions;
using SpotifyTrackView.Entity;

namespace SpotifyTrackView.Resources.Admin;

public class PlaylistResource
{
    public static object From(Playlist playlist, HttpRequest request)
    {
        return new
        {
            playlist.Id,
            playlist.SpotifyId,
            Influencer = InfluencerResource.From(playlist.Influencer),
            StatusName = playlist.Status.GetDisplayName(),
            StatusValue = playlist.Status,
            playlist.CreatedAt,
            playlist.UpdatedAt,
        };
    }
}