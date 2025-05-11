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
            Influencer = playlist is { Influencer: not null }
                ? InfluencerResource.From(playlist.Influencer)
                : null,
            StatusName = playlist.Status.GetDisplayName(),
            StatusValue = playlist.Status,
            playlist.CreatedAt,
            playlist.UpdatedAt,
        };
    }
}