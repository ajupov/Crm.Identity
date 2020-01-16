using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Identity.Profiles.Models;
using Crm.Identity.RefreshTokens.Models;
using Crm.Identity.Resources.Services;
using Crm.Identity.Scopes;
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

        public async Task<List<Claim>> GetByScopesAsync(List<string> scopes, Profile profile, CancellationToken ct)
        {
            var openIdClaims = GetOpenIdClaims(scopes, profile);
            var roleClaims = await GetRoleClaimsAsync(scopes, ct);

            return openIdClaims
                .Concat(roleClaims)
                .ToList();
        }

        public Task<List<Claim>> GetByRefreshTokenAsync(RefreshToken token, Profile profile, CancellationToken ct)
        {
            var claims = token.Claims
                .Select(x =>
                    new Claim
                    {
                        Type = x.Type,
                        Value = x.Value
                    })
                .ToList();

            return Task.FromResult(claims);
        }

        private static List<Claim> GetOpenIdClaims(ICollection<string> scopes, Profile profile)
        {
            var claims = new List<Claim>();

            if (scopes.Contains(ScopeNames.OpenId))
            {
                claims.Add(new Claim {Type = JwtDefaults.IdentifierClaimType, Value = profile.Id.ToString()});
            }

            return claims;
        }

        private async Task<List<Claim>> GetRoleClaimsAsync(IEnumerable<string> scopes, CancellationToken ct)
        {
            var roles = await _resourcesService.GetRolesByScopesAsync(scopes, ct);

            return roles
                .Select(x => new Claim {Type = ClaimTypes.Role, Value = x})
                .ToList();
        }
    }
}