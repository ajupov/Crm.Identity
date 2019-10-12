using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.OAuth.Models.Authorize;
using Ajupov.Identity.OAuth.Models.Tokens;

namespace Ajupov.Identity.OAuth.Services
{
    public interface IOAuthService
    {
        Task<PostAuthorizeResponse> AuthorizeAsync(
            string login,
            string password,
            string responseType,
            string redirectUri,
            string state,
            string userAgent,
            string ipAddress,
            List<string> scopes,
            CancellationToken ct);

        Task<TokenResponse> GetTokenAsync(
            string grandType,
            string code,
            string redirectUri,
            string userName,
            string password,
            string oldRefreshTokenValue,
            string userAgent,
            string ipAddress,
            List<string> oAuthClientScopes,
            CancellationToken ct);
    }
}