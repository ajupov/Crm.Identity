using System.Linq;
using System.Text.RegularExpressions;
using Ajupov.Identity.OAuth.Extensions;
using Ajupov.Identity.OAuthClients.Models;
using Ajupov.Utils.All.Password;
using Ajupov.Utils.All.String;

namespace Ajupov.Identity.OAuthClients.Validators
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

        public static bool IsMatchRedirectUri(this OAuthClient oAuthClient, string value)
        {
            return Regex.IsMatch(value, oAuthClient.RedirectUriPattern);
        }

        public static bool IsCorrectSecret(this OAuthClient oAuthClient, string value)
        {
            return Password.IsVerifiedPassword(value, oAuthClient.ClientSecret);
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
    }
}