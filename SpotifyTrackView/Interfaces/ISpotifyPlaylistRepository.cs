using System.Threading.Tasks;
using SpotifyTrackView.DataTransferObjects;

namespace SpotifyTrackView.Interfaces
{
    public interface ISpotifyPlaylistRepository
    {
        Task<PlaylistDto> GetPublicPlaylistAsync(string playlistId);
        Task<PlaylistDto> GetUserPlaylistAsync(string playlistId, string accessToken);
        Task<PlaylistCollectionDto> GetUserPlaylistsAsync(string accessToken, int limit = 20, int offset = 0);
    }
} 