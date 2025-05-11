using System.Collections.Generic;

namespace SpotifyTrackView.DataTransferObjects
{
    public class PlaylistDto
    {
        public bool Collaborative { get; set; }
        public string Description { get; set; }
        public ExternalUrls ExternalUrls { get; set; }
        public Followers Followers { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public List<Image> Images { get; set; }
        public string Name { get; set; }
        public Owner Owner { get; set; }
        public bool Public { get; set; }
        public string SnapshotId { get; set; }
        public Tracks Tracks { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
    }

    public class ExternalUrls
    {
        public string Spotify { get; set; }
    }

    public class Followers
    {
        public string Href { get; set; }
        public int Total { get; set; }
    }

    public class Image
    {
        public int? Height { get; set; }
        public string Url { get; set; }
        public int? Width { get; set; }
    }

    public class Owner
    {
        public string DisplayName { get; set; }
        public ExternalUrls ExternalUrls { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
    }

    public class Tracks
    {
        public string Href { get; set; }
        public List<TrackItem> Items { get; set; }
        public int Limit { get; set; }
        public string Next { get; set; }
        public int Offset { get; set; }
        public string Previous { get; set; }
        public int Total { get; set; }
    }

    public class TrackItem
    {
        public string AddedAt { get; set; }
        public Owner AddedBy { get; set; }
        public bool IsLocal { get; set; }
        public string PrimaryColor { get; set; }
        public Track Track { get; set; }
        public VideoThumbnail VideoThumbnail { get; set; }
    }

    public class Track
    {
        public string PreviewUrl { get; set; }
        public List<string> AvailableMarkets { get; set; }
        public bool Explicit { get; set; }
        public string Type { get; set; }
        public bool Episode { get; set; }
        public bool IsTrack { get; set; }
        public Album Album { get; set; }
        public List<Artist> Artists { get; set; }
        public int DiscNumber { get; set; }
        public int TrackNumber { get; set; }
        public int DurationMs { get; set; }
        public ExternalIds ExternalIds { get; set; }
        public ExternalUrls ExternalUrls { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Popularity { get; set; }
        public string Uri { get; set; }
        public bool IsLocal { get; set; }
    }

    public class Album
    {
        public List<string> AvailableMarkets { get; set; }
        public string Type { get; set; }
        public string AlbumType { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public List<Image> Images { get; set; }
        public string Name { get; set; }
        public string ReleaseDate { get; set; }
        public string ReleaseDatePrecision { get; set; }
        public string Uri { get; set; }
        public List<Artist> Artists { get; set; }
        public ExternalUrls ExternalUrls { get; set; }
        public int TotalTracks { get; set; }
    }

    public class Artist
    {
        public ExternalUrls ExternalUrls { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
    }

    public class ExternalIds
    {
        public string Isrc { get; set; }
    }

    public class VideoThumbnail
    {
        public string Url { get; set; }
    }

    public class PlaylistCollectionDto
    {
        public List<PlaylistDto> Items { get; set; }
        public int Total { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
    }
} 