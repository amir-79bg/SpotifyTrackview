using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Azure.Core;
using Microsoft.Extensions.Caching.Distributed;

namespace SpotifyTrackView.Repositories.Api;

public class SpotifyApi(string clientId, string clientSecret, IDistributedCache cache)
{
    private const string TokenKey = "spotify_token";

    public async Task<string?> GetToken()
    {
        var cachedToken = await cache.GetStringAsync(TokenKey);
        if (!string.IsNullOrEmpty(cachedToken))
        {
            return cachedToken;
        }
        
        HttpClient client = new HttpClient();

        string authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

        var requestContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });
        
        var response = await client.PostAsync("https://accounts.spotify.com/api/token", requestContent);
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        
        using var doc = JsonDocument.Parse(json);
        
        string accessToken = doc.RootElement.GetProperty("access_token").GetString();
        
        int expiresIn = doc.RootElement.GetProperty("expires_in").GetInt32();
    
        await cache.SetStringAsync(TokenKey, accessToken, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expiresIn - 60)
        });

        return accessToken;
    }
}