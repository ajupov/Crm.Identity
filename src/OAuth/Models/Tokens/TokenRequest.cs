using Ajupov.Identity.src.OAuth.Attributes.Validation;

namespace Ajupov.Identity.src.OAuth.Models.Tokens
{
    public class TokenRequest
    {
        [GrandTypeValidation]
        public string grant_type { get; set; }

        [ClientIdValidation]
        public string client_id { get; set; }

        [ClientSecretValidation]
        public string client_secret { get; set; }

        [CodeValidation]
        public string code { get; set; }

        [UsernameValidation]
        public string username { get; set; }

        [PasswordValidation]
        public string password { get; set; }

        [RefreshTokenValidation]
        public string refresh_token { get; set; }

        [RedirectUriValidation]
        public string redirect_uri { get; set; }
    }
}