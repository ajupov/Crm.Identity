using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.AccessTokens.Services;
using Ajupov.Identity.Claims.Services;
using Ajupov.Identity.Codes.Services;
using Ajupov.Identity.Identities.Extensions;
using Ajupov.Identity.Identities.Services;
using Ajupov.Identity.OAuth.Models.Authorize;
using Ajupov.Identity.OAuth.Models.Tokens;
using Ajupov.Identity.OAuth.Models.Types;
using Ajupov.Identity.Profiles.Services;
using Ajupov.Identity.RedirectUri.Services;
using Ajupov.Identity.RefreshTokens.Services;

namespace Ajupov.Identity.OAuth.Services
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
            string login,
            string password,
            string responseType,
            string redirectUri,
            string state,
            string userAgent,
            string ipAddress,
            List<string> scopes,
            CancellationToken ct)
        {
            var identityTypes = IdentityTypeExtensions.TypesWithPassword;
            var identity = await _identitiesService.GetVerifiedByKeyAndTypesAsync(login, identityTypes, ct);
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

                    return new PostAuthorizeResponse(callbackUri);
                }
                case ResponseType.Token:
                {
                    var accessToken = await _accessTokensService.CreateAsync(claims, ct);
                    var refreshToken =
                        await _refreshTokensService.CreateAsync(claims, profile, userAgent, ipAddress, ct);
                    var callbackUri = _callbackUriService.GetByTokens(redirectUri, state, accessToken, refreshToken);

                    return new PostAuthorizeResponse(callbackUri);
                }
                default:
                    throw new ArgumentOutOfRangeException(responseType);
            }
        }

        public async Task<TokenResponse> GetTokenAsync(
            string grandType,
            string code,
            string redirectUri,
            string userName,
            string password,
            string oldRefreshTokenValue,
            string userAgent,
            string ipAddress,
            List<string> oAuthClientScopes,
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
                        profileWithClaims.Profile, userAgent, ipAddress, ct);

                    return new TokenResponse(accessToken, refreshToken);
                }
                case GrandType.Password:
                {
                    var identityTypes = IdentityTypeExtensions.TypesWithPassword;
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

                    var claims = await _claimsService.GetByScopesAsync(oAuthClientScopes, profile, ct);
                    var accessToken = await _accessTokensService.CreateAsync(claims, ct);
                    var refreshToken =
                        await _refreshTokensService.CreateAsync(claims, profile, userAgent, ipAddress, ct);

                    return new TokenResponse(accessToken, refreshToken);
                }
                case GrandType.RefreshToken:
                {
                    var oldRefreshToken = await _refreshTokensService.GetByValueAsync(oldRefreshTokenValue, ct);
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
                        await _refreshTokensService.CreateAsync(claims, profile, userAgent, ipAddress, ct);

                    return new TokenResponse(accessToken, refreshToken);
                }
                default:
                    return new TokenResponse("Invalid grand type");
            }
        }
    }
}