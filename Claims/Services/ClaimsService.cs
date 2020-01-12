using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Identity.Profiles.Models;
using Crm.Identity.RefreshTokens.Models;
using Crm.Identity.Resources.Services;
using Claim = Crm.Identity.Claims.Models.Claim;

namespace Crm.Identity.Claims.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly IResourcesService _resourcesService;

        public ClaimsService(IResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
        }

        public async Task<List<Claim>> GetByScopesAsync(
            IEnumerable<string> scopes,
            Profile profile,
            CancellationToken ct)
        {
            var profileClaims = GetProfileClaims(profile);
            var apiClaims = await GetApiClaimsAsync(scopes, ct);

            return profileClaims
                .Concat(apiClaims)
                .ToList();
        }

        public Task<List<Claim>> GetByRefreshTokenAsync(
            RefreshToken refreshToken,
            Profile profile,
            CancellationToken ct)
        {
            var profileClaims = GetProfileClaims(profile);
            var apiClaims = refreshToken.Claims.Select(x => new Claim {Type = x.Type, Value = x.Value});

            return Task.FromResult(
                profileClaims
                    .Concat(apiClaims)
                    .ToList());
        }

        private static List<Claim> GetProfileClaims(Profile profile)
        {
            return new List<Claim>
            {
                new Claim {Type = JwtDefaults.IdentifierClaimType, Value = profile.Id.ToString()}
            };
        }

        private async Task<List<Claim>> GetApiClaimsAsync(IEnumerable<string> scopes, CancellationToken ct)
        {
            var roles = await _resourcesService.GetRolesByScopesAsync(scopes, ct);

            return roles
                .Select(x => new Claim {Type = ClaimTypes.Role, Value = x})
                .ToList();
        }
    }
}