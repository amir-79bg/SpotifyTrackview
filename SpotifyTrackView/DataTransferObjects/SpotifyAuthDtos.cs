namespace SpotifyTrackView.DataTransferObjects
{
    public class AuthResponseDto
    {
        public string AuthorizationUrl { get; set; }
        public string State { get; set; }
    }

    public class TokenResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string Scope { get; set; }
    }
} 