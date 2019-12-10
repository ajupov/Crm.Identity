using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Phone;
using Crm.Identity.Areas.AccessTokens.Services;
using Crm.Identity.Areas.Claims.Services;
using Crm.Identity.Areas.Codes.Services;
using Crm.Identity.Areas.Identities.Extensions;
using Crm.Identity.Areas.Identities.Models;
using Crm.Identity.Areas.Identities.Services;
using Crm.Identity.Areas.OAuth.Models.Authorize;
using Crm.Identity.Areas.OAuth.Models.Tokens;
using Crm.Identity.Areas.OAuth.Models.Types;
using Crm.Identity.Areas.Profiles.Services;
using Crm.Identity.Areas.RedirectUri.Services;
using Crm.Identity.Areas.RefreshTokens.Services;

namespace Crm.Identity.Areas.OAuth.Services
{
    public class OAuthService : IOAuthService
    {
        private readonly IProfilesService _profilesService;
        private readonly IIdentitiesService _identitiesService;
        private readonly IClaimsService _claimsService;
        private readonly ICodesService _codesService;
        private readonly IAccessTokensService _accessTokensService;
        private readonly IRefreshTokensService _refreshTokensService;
        private readonly ICallbackUriService _callbackUriService;

        public OAuthService(
            IProfilesService profilesService,
            IIdentitiesService identitiesService,
            IClaimsService claimsService,
            ICodesService codesService,
            IAccessTokensService accessTokensService,
            IRefreshTokensService refreshTokensService,
            ICallbackUriService callbackUriService)
        {
            _profilesService = profilesService;
            _identitiesService = identitiesService;
            _claimsService = claimsService;
            _codesService = codesService;
            _accessTokensService = accessTokensService;
            _refreshTokensService = refreshTokensService;
            _callbackUriService = callbackUriService;
        }

        public async Task<PostAuthorizeResponse> AuthorizeAsync(
            string country,
            string key,
            string password,
            string responseType,
            string redirectUri,
            string state,
            string ipAddress,
            string userAgent,
            IEnumerable<string> scopes,
            CancellationToken ct)
        {
            var identityTypes = IdentityTypeExtensions.TypesWithPassword;
            var phoneIdentityType = new[] {IdentityType.PhoneAndPassword};

            var identity = await _identitiesService.GetVerifiedByKeyAndTypesAsync(key, identityTypes, ct) ??
                           await _identitiesService.GetVerifiedByKeyAndTypesAsync(key.GetPhoneWithoutPrefixes(country),
                               phoneIdentityType, ct);

            if (identity == null)
            {
                return new PostAuthorizeResponse(redirectUri, true);
            }

            var profile = await _profilesService.GetAsync(identity.ProfileId, ct);
            if (profile == null)
            {
                return new PostAuthorizeResponse(redirectUri, true);
            }

            var isPasswordCorrect = _identitiesService.IsPasswordCorrect(identity, password);
            if (!isPasswordCorrect)
            {
                return new PostAuthorizeResponse(redirectUri, true);
            }

            var claims = await _claimsService.GetByScopesAsync(scopes, profile, ct);

            switch (responseType)
            {
                case ResponseType.Code:
                {
                    var code = _codesService.Create(profile, claims);
                    var callbackUri = _callbackUriService.GetByCode(redirectUri, state, code);

                    return new PostAuthorizeResponse(callbackUri, false);
                }
                case ResponseType.Token:
                {
                    var accessToken = await _accessTokensService.CreateAsync(claims, ct);
                    var refreshToken =
                        await _refreshTokensService.CreateAsync(claims, profile, ipAddress, userAgent, ct);
                    var callbackUri = _callbackUriService.GetByTokens(redirectUri, state, accessToken, refreshToken);

                    return new PostAuthorizeResponse(callbackUri, false);
                }
                default:
                    throw new ArgumentOutOfRangeException(responseType);
            }
        }

        public async Task<TokenResponse> GetTokenAsync(
            string grandType,
            string code,
            string userName,
            string password,
            string oldRefreshTokenValue,
            string ipAddress,
            string userAgent,
            IEnumerable<string> scopes,
            CancellationToken ct)
        {
            switch (grandType)
            {
                case GrandType.AuthorizationCode:
                {
                    var profileWithClaims = _codesService.Get(code);
                    if (profileWithClaims == null)
                    {
                        return new TokenResponse("Invalid code");
                    }

                    var accessToken = await _accessTokensService.CreateAsync(profileWithClaims.Claims, ct);
                    var refreshToken = await _refreshTokensService.CreateAsync(profileWithClaims.Claims,
                        profileWithClaims.Profile, ipAddress, userAgent, ct);

                    return new TokenResponse(accessToken, refreshToken);
                }
                case GrandType.Password:
                {
                    var identityTypes = new[] {IdentityType.LoginAndPassword};
                    var identity = await _identitiesService.GetVerifiedByKeyAndTypesAsync(userName, identityTypes, ct);
                    if (identity == null)
                    {
                        return new TokenResponse("Invalid credentials");
                    }

                    var profile = await _profilesService.GetAsync(identity.ProfileId, ct);
                    if (profile == null)
                    {
                        return new TokenResponse("Invalid credentials");
                    }

                    var isPasswordCorrect = _identitiesService.IsPasswordCorrect(identity, password);
                    if (!isPasswordCorrect)
                    {
                        return new TokenResponse("Invalid credentials");
                    }

                    var claims = await _claimsService.GetByScopesAsync(scopes, profile, ct);
                    var accessToken = await _accessTokensService.CreateAsync(claims, ct);
                    var refreshToken =
                        await _refreshTokensService.CreateAsync(claims, profile, ipAddress, userAgent, ct);

                    return new TokenResponse(accessToken, refreshToken);
                }
                case GrandType.RefreshToken:
                {
                    var oldRefreshToken = await _refreshTokensService.GetByValueAsync(oldRefreshTokenValue, ct);
                    if (oldRefreshToken == null)
                    {
                        return new TokenResponse("Invalid refresh token");
                    }

                    if (oldRefreshToken.ExpirationDateTime < DateTime.UtcNow)
                    {
                        return new TokenResponse("Refresh token is expired");
                    }

                    await _refreshTokensService.SetExpiredAsync(oldRefreshToken.Id, ct);

                    var profile = await _profilesService.GetAsync(oldRefreshToken.ProfileId, ct);
                    if (profile == null)
                    {
                        return new TokenResponse("Invalid refresh token");
                    }

                    var claims = await _claimsService.GetByRefreshTokenAsync(oldRefreshToken, profile, ct);
                    var accessToken = await _accessTokensService.CreateAsync(claims, ct);
                    var refreshToken =
                        await _refreshTokensService.CreateAsync(claims, profile, ipAddress, userAgent, ct);

                    return new TokenResponse(accessToken, refreshToken);
                }
                default:
                    return new TokenResponse("Invalid grand type");
            }
        }
    }
}