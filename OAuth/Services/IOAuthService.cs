using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.OAuth.Models.Authorize;
using Crm.Identity.OAuth.Models.Tokens;
using Crm.Identity.OAuth.Models.UserInfo;

namespace Crm.Identity.OAuth.Services
{
    public interface IOAuthService
    {
        Task<PostAuthorizeResponse> AuthorizeAsync(
            string country,
            string key,
            string password,
            string responseType,
            string redirectUri,
            string state,
            string ipAddress,
            string userAgent,
            List<string> scopes,
            string audience,
            CancellationToken ct);

        Task<UserInfoResponse> GetUserInfoAsync(string accessToken, CancellationToken ct);

        Task<TokenResponse> GetTokenAsync(
            string grandType,
            string code,
            string userName,
            string password,
            string oldRefreshTokenValue,
            string ipAddress,
            string userAgent,
            List<string> clientScopes,
            string audience,
            CancellationToken ct);
    }
}