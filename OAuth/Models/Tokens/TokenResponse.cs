using System;

namespace Crm.Identity.OAuth.Models.Tokens
{
    public class TokenResponse
    {
        public TokenResponse(string accessToken, string refreshToken)
        {
            access_token = accessToken;
            refresh_token = refreshToken;
            token_type = "bearer";
            expires_in = (int) TimeSpan.FromMinutes(60).TotalSeconds;
        }

        public TokenResponse(string error)
        {
            this.error = error;
        }

        public string access_token { get; set; }

        public string refresh_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }

        public string error { get; set; }
    }
}