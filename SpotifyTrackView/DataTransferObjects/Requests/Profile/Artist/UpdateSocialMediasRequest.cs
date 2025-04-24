using System.ComponentModel.DataAnnotations;

namespace SpotifyTrackView.DataTransferObjects.Requests.Profile.Artist;

public class UpdateSocialMediasRequest
{
    [Url] public string? SpotifyUrl { get; set; }
    [Url] public string? SoundcloudUrl { get; set; }
    [Url] public string? YoutubeUrl { get; set; }
    [Url] public string? InstagramUrl { get; set; }
    [Url] public string? FacebookUrl { get; set; }
    [Url] public string? TiktokUrl { get; set; }
    [Url] public string? WebsiteUrl { get; set; }
}