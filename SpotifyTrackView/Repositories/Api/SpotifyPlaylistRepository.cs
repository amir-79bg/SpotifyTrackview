using System.Net.Http.Headers;
using Newtonsoft.Json;
using SpotifyTrackView.DataTransferObjects;
using SpotifyTrackView.Interfaces;

namespace SpotifyTrackView.Repositories.Api
{
    public class SpotifyPlaylistRepository : ISpotifyPlaylistRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://api.spotify.com/v1";
        private readonly SpotifyApi _spotifyApi;

        public SpotifyPlaylistRepository(HttpClient httpClient, SpotifyApi spotifyApi)
        {
            _httpClient = httpClient;
            _spotifyApi = spotifyApi;
        }

        public async Task<PlaylistDto> GetPublicPlaylistAsync(string playlistId)
        {
            var accessToken = await _spotifyApi.GetToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Failed to get Spotify access token");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"{_baseUrl}/playlists/{playlistId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var playlist = JsonConvert.DeserializeObject<PlaylistDto>(content);
            return playlist;
        }

        public async Task<PlaylistDto> GetUserPlaylistAsync(string playlistId, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"{_baseUrl}/playlists/{playlistId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var playlist = JsonConvert.DeserializeObject<PlaylistDto>(content);
            return playlist;
        }

        public async Task<PlaylistCollectionDto> GetUserPlaylistsAsync(string accessToken, int limit = 20, int offset = 0)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"{_baseUrl}/me/playlists?limit={limit}&offset={offset}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var playlists = JsonConvert.DeserializeObject<PlaylistCollectionDto>(content);
            return playlists;
        }
    }
} 