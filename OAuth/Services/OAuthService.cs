using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Identities.Extensions;
using Crm.Identity.Identities.Models;
using Crm.Identity.Identities.Requests;
using Crm.Identity.Identities.Services;
using Crm.Identity.OAuth.Models.Authorize;
using Crm.Identity.OAuth.Models.Tokens;
using Crm.Identity.OAuth.Models.Types;
using Crm.Identity.OAuth.Options;
using Crm.Identity.Profiles.Models;
using Crm.Identity.Profiles.Services;
using Infrastructure.All.Generator;
using Infrastructure.All.HotStorage.HotStorage;
using Infrastructure.All.Http;
using Microsoft.IdentityModel.Tokens;

namespace Crm.Identity.OAuth.Services
{
    public class OAuthService : IOAuthService
    {
        private readonly IHotStorage _hotStorage;
        private readonly IProfilesService _profilesService;
        private readonly IIdentitiesService _identitiesService;
        private readonly IIdentityTokensService _identityTokensService;

        public OAuthService(
            IHotStorage hotStorage,
            IProfilesService profilesService,
            IIdentitiesService identitiesService,
            IIdentityTokensService identityTokensService)
        {
            _hotStorage = hotStorage;
            _profilesService = profilesService;
            _identitiesService = identitiesService;
            _identityTokensService = identityTokensService;
        }

        public bool IsAuthorized(ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            return claim != null;
        }

        public async Task<PostAuthorizeResponse> AuthorizeAsync(
            PostAuthorizeRequest request,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            var identityTypes = IdentityTypeExtensions.TypesWithPassword;
            var identity = await _identitiesService.GetByKeyAndTypesAsync(request.Login, identityTypes, ct);
            if (identity == null)
            {
                return PostAuthorizeResponse.ErrorResponse;
            }

            var user = await _profilesService.GetAsync(identity.ProfileId, ct);
            if (user == null)
            {
                return PostAuthorizeResponse.ErrorResponse;
            }

            var isPasswordCorrect = _identitiesService.IsPasswordCorrect(identity, request.Password);
            if (!isPasswordCorrect)
            {
                return PostAuthorizeResponse.ErrorResponse;
            }

            var parameters = await GetRedirectUrlParametersAsync(request, user, identity, userAgent, ipAddress, ct);
            var redirectUri = request.redirect_uri.AddParameters(parameters);

            return new PostAuthorizeResponse(redirectUri);
        }

        public async Task<TokenResponse> GetTokenAsync(
            TokenRequest request,
            ClaimsPrincipal claimsPrincipal,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            switch (request.GrantType)
            {
                case GrandType.AuthorizationCode:
                    var identityId = _hotStorage.GetValue<Guid>(request.Code);
                    var identityByCode = await _identitiesService.GetAsync(identityId, ct);
                    if (identityByCode == null)
                    {
                        return new TokenResponse("Invalid code");
                    }

                    var userByCode = await _profilesService.GetAsync(identityByCode.ProfileId, ct);
                    if (userByCode == null)
                    {
                        return new TokenResponse("Invalid code");
                    }

                    var accessTokenByCode =
                        await CreateAccessTokenAsync(request.RedirectUri, userByCode, identityByCode, userAgent,
                            ipAddress, ct);
                    var refreshTokenByCode = await CreateRefreshTokenAsync(identityByCode, userAgent, ipAddress, ct);

                    return new TokenResponse(accessTokenByCode.Value, refreshTokenByCode.Value, "bearer",
                        TimeSpan.FromMinutes(30).Seconds);

                case GrandType.Password:
                    var identityTypes = IdentityTypeExtensions.TypesWithPassword;
                    var identityByPassword =
                        await _identitiesService.GetByKeyAndTypesAsync(request.Username, identityTypes, ct);
                    if (identityByPassword == null)
                    {
                        return new TokenResponse("Invalid credentials");
                    }

                    var userByPassword = await _profilesService.GetAsync(identityByPassword.ProfileId, ct);
                    if (userByPassword == null)
                    {
                        return new TokenResponse("Invalid credentials");
                    }

                    var isPasswordCorrect = _identitiesService.IsPasswordCorrect(identityByPassword, request.Password);
                    if (!isPasswordCorrect)
                    {
                        return new TokenResponse("Invalid credentials");
                    }

                    var accessTokenByPassword =
                        await CreateAccessTokenAsync(request.RedirectUri, userByPassword, identityByPassword, userAgent,
                            ipAddress, ct);
                    var refreshTokenByPassword =
                        await CreateRefreshTokenAsync(identityByPassword, userAgent, ipAddress, ct);

                    return new TokenResponse(accessTokenByPassword.Value, refreshTokenByPassword.Value, "bearer",
                        TimeSpan.FromMinutes(30).Seconds);

                case GrandType.RefreshToken:

                    var oldRefreshToken = await _identityTokensService.GetByValueAsync(IdentityTokenType.RefreshToken,
                        request.RefreshToken, ct);

                    if (oldRefreshToken.ExpirationDateTime > DateTime.UtcNow)
                    {
                        return new TokenResponse("Refresh token is expired");
                    }

                    var identityByRefresh = await _identitiesService.GetAsync(oldRefreshToken.IdentityId, ct);
                    var userByRefresh = await _profilesService.GetAsync(identityByRefresh.ProfileId, ct);

                    var accessTokenByRefresh =
                        await CreateAccessTokenAsync(request.RedirectUri, userByRefresh, identityByRefresh, userAgent,
                            ipAddress, ct);
                    var refreshTokenByRefresh =
                        await CreateRefreshTokenAsync(identityByRefresh, userAgent, ipAddress, ct);

                    return new TokenResponse(accessTokenByRefresh.Value, refreshTokenByRefresh.Value, "bearer",
                        TimeSpan.FromMinutes(30).Seconds);

                default:
                    return new TokenResponse("Invalid grand type");
            }
        }

        private async Task<(string key, object value)[]> GetRedirectUrlParametersAsync(
            PostAuthorizeRequest request,
            Profile profile,
            Identities.Models.Identity identity,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            switch (request.response_type)
            {
                case ResponseType.Code:
                    return GetCodeUriParameters(identity);
                case ResponseType.Token:
                    return await GetTokensUriParametersAsync(request, profile, identity, userAgent, ipAddress, ct);
                default:
                    throw new ArgumentOutOfRangeException(request.response_type);
            }
        }

        private (string key, object value)[] GetCodeUriParameters(Identities.Models.Identity identity)
        {
            var code = Generator.GenerateAlphaNumericString(8);

            _hotStorage.SetValue(code, identity.Id, TimeSpan.FromMinutes(10));

            return new (string key, object value)[]
            {
                ("code", code)
            };
        }

        private async Task<(string key, object value)[]> GetTokensUriParametersAsync(
            PostAuthorizeRequest request,
            Profile profile,
            Identities.Models.Identity identity,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            var accessToken =
                await CreateAccessTokenAsync(request.redirect_uri, profile, identity, userAgent, ipAddress, ct);
            var refreshToken = await CreateRefreshTokenAsync(identity, userAgent, ipAddress, ct);

            return new (string key, object value)[]
            {
                ("token_type", "bearer"),
                ("access_token", accessToken.Value),
                ("refresh_token", refreshToken.Value),
                ("expires_in", TimeSpan.FromDays(1).TotalSeconds)
            };
        }

        private async Task<IdentityToken> CreateAccessTokenAsync(
            string redirectUri,
            Profile profile,
            Identities.Models.Identity identity,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var audience = new Uri(redirectUri).Host;
            var credentials = new SigningCredentials(AuthOptions.GetKey(), SecurityAlgorithms.HmacSha256);
            var claims = await GetClaimsAsync(profile, ct);

            var jwt = new JwtSecurityToken("Identity", audience, notBefore: now, claims: claims,
                expires: now.AddMinutes(30), signingCredentials: credentials);

            var accessToken = new IdentityToken
            {
                Id = Guid.NewGuid(),
                IdentityId = identity.Id,
                Type = IdentityTokenType.AccessToken,
                Value = new JwtSecurityTokenHandler().WriteToken(jwt),
                CreateDateTime = DateTime.UtcNow,
                ExpirationDateTime = DateTime.UtcNow.AddMinutes(30),
                UserAgent = userAgent,
                IpAddress = ipAddress
            };

            await _identityTokensService.CreateAsync(accessToken, ct);

            return accessToken;
        }

        private async Task<IdentityToken> CreateRefreshTokenAsync(
            Identities.Models.Identity identity,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            var refreshToken = new IdentityToken
            {
                Id = Guid.NewGuid(),
                IdentityId = identity.Id,
                Type = IdentityTokenType.RefreshToken,
                Value = Generator.GenerateAlphaNumericString(32),
                CreateDateTime = DateTime.UtcNow,
                ExpirationDateTime = DateTime.UtcNow.AddDays(60),
                UserAgent = userAgent,
                IpAddress = ipAddress
            };

            await _identityTokensService.CreateAsync(refreshToken, ct);

            return refreshToken;
        }

        private async Task<Claim[]> GetClaimsAsync(Profile profile, CancellationToken ct)
        {
            var parameter = new IdentitiesGetPagedListRequest
            {
                ProfileId = profile.Id,
                IsVerified = true,
                Types = new List<IdentityType>
                {
                    IdentityType.EmailAndPassword,
                    IdentityType.PhoneAndPassword
                }
            };
            var allIdentities = await _identitiesService.GetPagedListAsync(parameter, ct);
            var email = allIdentities.FirstOrDefault(x => x.Type == IdentityType.EmailAndPassword)?.Key;
            var phone = allIdentities.FirstOrDefault(x => x.Type == IdentityType.PhoneAndPassword)?.Key;

            return new[]
            {
                new Claim(ClaimTypes.NameIdentifier, profile.Id.ToString()),
                new Claim(ClaimTypes.Surname, profile.Surname),
                new Claim(ClaimTypes.Name, profile.Name),
                new Claim(ClaimTypes.DateOfBirth, profile.BirthDate?.ToString("dd.MM.yyyy")),
                new Claim(ClaimTypes.Gender, profile.Gender.ToString().ToLower()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.HomePhone, phone)
            };
        }
    }
}