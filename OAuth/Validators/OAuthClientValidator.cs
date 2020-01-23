using System.Linq;
using System.Text.RegularExpressions;
using Ajupov.Utils.All.String;
using Crm.Identity.OAuth.Extensions;
using Crm.Identity.OAuth.Models.Authorize;
using Crm.Identity.OAuth.Models.Register;
using Crm.Identity.OAuth.Models.Tokens;
using Crm.Identity.OAuth.Models.Types;
using Crm.Identity.OAuthClients.Models;
using Crm.Identity.Scopes;
using PasswordUtils = Ajupov.Utils.All.Password.Password;

namespace Crm.Identity.OAuth.Validators
{
    public static class OAuthClientValidator
    {
        public static bool IsValid(this OAuthClient oAuthClient)
        {
            return oAuthClient?.Scopes?.Any() == true && !oAuthClient.IsLocked && !oAuthClient.IsDeleted;
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
                .ToScopeList()
                .ToHashSet();

            return requireScopes.IsSubsetOf(oAuthClientScopes) && requireScopes.Contains(ScopeNames.OpenId);
        }

        private static bool IsMatchRedirectUri(OAuthClient oAuthClient, string redirectUri)
        {
            return Regex.IsMatch(redirectUri, oAuthClient.RedirectUriPattern);
        }
    }
}