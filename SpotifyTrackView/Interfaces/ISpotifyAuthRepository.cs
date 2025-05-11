using System.Threading.Tasks;
using SpotifyTrackView.DataTransferObjects;

namespace SpotifyTrackView.Interfaces
{
    public interface ISpotifyAuthRepository
    {
        Task<AuthResponseDto> GetAuthorizationUrlAsync(string state);
        Task<TokenResponseDto> ExchangeCodeForTokenAsync(string code);
        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string accessToken);
    }
} 