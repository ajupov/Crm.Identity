using System;
using Infrastructure.All.Http;

namespace Crm.Identity.RedirectUri.Services
{
    public class CallbackUriService : ICallbackUriService
    {
        public string GetByCode(string redirectUri, string state, string code)
        {
            var parameters = new (string key, object value)[]
            {
                ("state", state),
                ("code", code)
            };

            return redirectUri.AddParameters(parameters);
        }

        public string GetByTokens(string redirectUri, string state, string accessToken, string refreshToken)
        {
            var parameters = new (string key, object value)[]
            {
                ("state", state),
                ("token_type", "bearer"),
                ("access_token", accessToken),
                ("refresh_token", refreshToken),
                ("expires_in", TimeSpan.FromDays(1).TotalSeconds)
            };

            return redirectUri.AddParameters(parameters);
        }
    }
}