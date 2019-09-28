using Ajupov.Utils.All.String;
using Newtonsoft.Json;

namespace Crm.Identity.OAuth.Models.Tokens
{
    public class TokenResponse
    {
        public TokenResponse(string accessToken, string refreshToken, string tokenType, int expiresIn)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            TokenType = tokenType;
            ExpiresIn = expiresIn;
        }

        public TokenResponse(string error)
        {
            Error = error;
        }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        public bool HasError => !Error.IsEmpty();
    }
}