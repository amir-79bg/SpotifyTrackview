using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpotifyTrackView.DataTransferObjects;
using SpotifyTrackView.Interfaces;

namespace SpotifyTrackView.Repositories.Api
{
    public class SpotifyAuthRepository : ISpotifyAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;

        public SpotifyAuthRepository(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            
            _clientId = _configuration["Spotify:ClientId"] ?? throw new ArgumentException();
            _clientSecret = _configuration["Spotify:ClientSecret"] ?? throw new ArgumentException();
            _redirectUri = _configuration["Spotify:RedirectUri"] ?? throw new ArgumentException();
           
        }

        public async Task<AuthResponseDto> GetAuthorizationUrlAsync(string state)
        {
            var scopes = "playlist-read-private playlist-read-collaborative user-read-private user-read-email";
            var encodedRedirectUri = Uri.EscapeDataString(_redirectUri);
            var encodedScopes = Uri.EscapeDataString(scopes);
            
            var authUrl = $"https://accounts.spotify.com/authorize?client_id={_clientId}&response_type=code&redirect_uri={encodedRedirectUri}&scope={encodedScopes}&state={state}";
            
            return new AuthResponseDto
            {
                AuthorizationUrl = authUrl,
                State = state
            };
        }

        public async Task<TokenResponseDto> ExchangeCodeForTokenAsync(string code)
        {
            var tokenEndpoint = "https://accounts.spotify.com/api/token";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", _redirectUri)
            });

            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);

            var response = await _httpClient.PostAsync(tokenEndpoint, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenResponseDto>(responseContent);
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var tokenEndpoint = "https://accounts.spotify.com/api/token";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            });

            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);

            var response = await _httpClient.PostAsync(tokenEndpoint, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenResponseDto>(responseContent);
        }

        public async Task<bool> ValidateTokenAsync(string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.GetAsync("https://api.spotify.com/v1/me");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
} 