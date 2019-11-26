using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.OAuth.Models.Authorize;
using Crm.Identity.OAuth.Models.Tokens;

namespace Crm.Identity.OAuth.Services
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
            IEnumerable<string> scopes,
            CancellationToken ct);

        Task<TokenResponse> GetTokenAsync(
            string grandType,
            string code,
            string userName,
            string password,
            string oldRefreshTokenValue,
            string userAgent,
            string ipAddress,
            IEnumerable<string> scopes,
            CancellationToken ct);
    }
}