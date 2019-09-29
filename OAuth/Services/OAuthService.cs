using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Identities.Extensions;
using Ajupov.Identity.Identities.Models;
using Ajupov.Identity.Identities.Requests;
using Ajupov.Identity.Identities.Services;
using Ajupov.Identity.OAuth.Models.Authorize;
using Ajupov.Identity.OAuth.Models.Tokens;
using Ajupov.Identity.OAuth.Models.Types;
using Ajupov.Identity.OAuth.Options;
using Ajupov.Identity.Profiles.Models;
using Ajupov.Identity.Profiles.Services;
using Ajupov.Infrastructure.All.HotStorage.HotStorage;
using Infrastructure.All.Generator;
using Infrastructure.All.Http;
using Microsoft.IdentityModel.Tokens;

namespace Ajupov.Identity.OAuth.Services
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
            string login,
            string password,
            bool isRemember,
            string responseType,
            string redirectUri,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            var identityTypes = IdentityTypeExtensions.TypesWithPassword;
            var identity = await _identitiesService.GetByKeyAndTypesAsync(login, identityTypes, ct);
            if (identity == null)
            {
                return new PostAuthorizeResponse(redirectUri, true);
            }

            var user = await _profilesService.GetAsync(identity.ProfileId, ct);
            if (user == null)
            {
                return new PostAuthorizeResponse(redirectUri, true);
            }

            var isPasswordCorrect = _identitiesService.IsPasswordCorrect(identity, password);
            if (!isPasswordCorrect)
            {
                return new PostAuthorizeResponse(redirectUri, true);
            }

            var parameters =
                await GetRedirectUrlParametersAsync(responseType, redirectUri, user, identity, userAgent, ipAddress,
                    ct);

            return new PostAuthorizeResponse(redirectUri.AddParameters(parameters));
        }

        public async Task<TokenResponse> GetTokenAsync(
            string grandType,
            string code,
            string redirectUri,
            string userName,
            string password,
            string refreshToken,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            switch (grandType)
            {
                case GrandType.AuthorizationCode:
                    var identityId = _hotStorage.GetValue<Guid>(code);
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

                    var accessTokenByCode = await CreateAccessTokenAsync(redirectUri, userByCode, identityByCode,
                        userAgent, ipAddress, ct);
                    var refreshTokenByCode = await CreateRefreshTokenAsync(identityByCode, userAgent, ipAddress, ct);

                    return new TokenResponse(accessTokenByCode.Value, refreshTokenByCode.Value, "bearer",
                        TimeSpan.FromMinutes(30).Seconds);

                case GrandType.Password:
                    var identityTypes = IdentityTypeExtensions.TypesWithPassword;
                    var identityByPassword =
                        await _identitiesService.GetByKeyAndTypesAsync(userName, identityTypes, ct);
                    if (identityByPassword == null)
                    {
                        return new TokenResponse("Invalid credentials");
                    }

                    var userByPassword = await _profilesService.GetAsync(identityByPassword.ProfileId, ct);
                    if (userByPassword == null)
                    {
                        return new TokenResponse("Invalid credentials");
                    }

                    var isPasswordCorrect = _identitiesService.IsPasswordCorrect(identityByPassword, password);
                    if (!isPasswordCorrect)
                    {
                        return new TokenResponse("Invalid credentials");
                    }

                    var accessTokenByPassword = await CreateAccessTokenAsync(redirectUri, userByPassword,
                        identityByPassword, userAgent, ipAddress, ct);
                    var refreshTokenByPassword =
                        await CreateRefreshTokenAsync(identityByPassword, userAgent, ipAddress, ct);

                    return new TokenResponse(accessTokenByPassword.Value, refreshTokenByPassword.Value, "bearer",
                        TimeSpan.FromMinutes(30).Seconds);

                case GrandType.RefreshToken:

                    var oldRefreshToken = await _identityTokensService.GetByValueAsync(IdentityTokenType.RefreshToken,
                        refreshToken, ct);

                    if (oldRefreshToken.ExpirationDateTime > DateTime.UtcNow)
                    {
                        return new TokenResponse("Refresh token is expired");
                    }

                    var identityByRefresh = await _identitiesService.GetAsync(oldRefreshToken.IdentityId, ct);
                    var userByRefresh = await _profilesService.GetAsync(identityByRefresh.ProfileId, ct);

                    var accessTokenByRefresh =
                        await CreateAccessTokenAsync(redirectUri, userByRefresh, identityByRefresh, userAgent,
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
            string responseType,
            string redirectUri,
            Profile profile,
            Identities.Models.Identity identity,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            switch (responseType)
            {
                case ResponseType.Code:
                    return GetCodeUriParameters(identity);
                case ResponseType.Token:
                    return await GetTokensUriParametersAsync(redirectUri, profile, identity, userAgent, ipAddress, ct);
                default:
                    throw new ArgumentOutOfRangeException(responseType);
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
            string redirectUri,
            Profile profile,
            Identities.Models.Identity identity,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            var accessToken = await CreateAccessTokenAsync(redirectUri, profile, identity, userAgent, ipAddress, ct);
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