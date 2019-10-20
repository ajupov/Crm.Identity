using System.Linq;
using System.Text.RegularExpressions;
using Ajupov.Identity.src.OAuth.Extensions;
using Ajupov.Identity.src.OAuth.Models.Authorize;
using Ajupov.Identity.src.OAuth.Models.Register;
using Ajupov.Identity.src.OAuth.Models.Tokens;
using Ajupov.Identity.src.OAuth.Models.Types;
using Ajupov.Identity.src.OAuthClients.Models;
using Ajupov.Utils.All.String;
using PasswordUtils = Ajupov.Utils.All.Password.Password;

namespace Ajupov.Identity.src.OAuth.Validators
{
    public static class OAuthClientValidator
    {
        public static bool IsValid(this OAuthClient oAuthClient)
        {
            return oAuthClient?.Scopes?.Any() == true &&
                   !oAuthClient.IsLocked &&
                   !oAuthClient.IsDeleted &&
                   !oAuthClient.ClientSecret.IsEmpty() &&
                   !oAuthClient.RedirectUriPattern.IsEmpty();
        }

        public static bool IsMatchRedirectUri(this OAuthClient oAuthClient, GetAuthorizeRequest request)
        {
            return IsMatchRedirectUri(oAuthClient, request.redirect_uri);
        }

        public static bool IsMatchRedirectUri(this OAuthClient oAuthClient, PostAuthorizeRequest request)
        {
            return IsMatchRedirectUri(oAuthClient, request.redirect_uri);
        }

        public static bool IsMatchRedirectUri(this OAuthClient oAuthClient, GetRegisterRequest request)
        {
            return IsMatchRedirectUri(oAuthClient, request.redirect_uri);
        }

        public static bool IsMatchRedirectUri(this OAuthClient oAuthClient, PostRegisterRequest request)
        {
            return IsMatchRedirectUri(oAuthClient, request.redirect_uri);
        }

        public static bool IsMatchRedirectUri(this OAuthClient oAuthClient, TokenRequest request)
        {
            return request.grant_type != GrandType.AuthorizationCode ||
                   IsMatchRedirectUri(oAuthClient, request.redirect_uri);
        }

        public static bool IsCorrectSecret(this OAuthClient oAuthClient, TokenRequest request)
        {
            return request.grant_type != GrandType.AuthorizationCode ||
                   PasswordUtils.IsVerifiedPassword(request.client_secret, oAuthClient.ClientSecret);
        }

        public static bool IsScopesInclude(this OAuthClient oAuthClient, string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }

            var oAuthClientScopes = oAuthClient.Scopes
                .Select(x => x.Value)
                .ToHashSet();

            var requireScopes = value
                .ToList()
                .ToHashSet();

            return requireScopes.IsSubsetOf(oAuthClientScopes);
        }

        private static bool IsMatchRedirectUri(OAuthClient oAuthClient, string redirectUri)
        {
            return Regex.IsMatch(redirectUri, oAuthClient.RedirectUriPattern);
        }
    }
}