using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Identities.Models;
using Ajupov.Identity.Identities.Requests;
using Ajupov.Identity.Identities.Services;
using Ajupov.Identity.Profiles.Models;
using Ajupov.Identity.RefreshTokens.Models;
using Ajupov.Identity.Resources.Services;

namespace Ajupov.Identity.OAuth.Services.Claims
{
    public class ClaimsService : IClaimsService
    {
        private readonly IIdentitiesService _identitiesService;
        private readonly IResourcesService _resourcesService;

        public ClaimsService(IIdentitiesService identitiesService, IResourcesService resourcesService)
        {
            _identitiesService = identitiesService;
            _resourcesService = resourcesService;
        }

        public async Task<List<Claim>> GetByScopesAsync(List<string> scopes, Profile profile, CancellationToken ct)
        {
            var profileClaims = await GetProfileClaimsAsync(profile, ct);
            var apiClaims = await GetApiClaimsAsync(scopes, ct);

            return profileClaims
                .Concat(apiClaims)
                .ToList();
        }

        public async Task<List<Claim>> GetByRefreshTokenAsync(
            RefreshToken refreshToken,
            Profile profile,
            CancellationToken ct)
        {
            var profileClaims = await GetProfileClaimsAsync(profile, ct);
            var apiClaims = refreshToken.Claims.Select(x => new Claim(x.Type, x.Value));

            return profileClaims
                .Concat(apiClaims)
                .ToList();
        }

        private async Task<Claim[]> GetProfileClaimsAsync(Profile profile, CancellationToken ct)
        {
            var identityTypes = new List<IdentityType>
            {
                IdentityType.EmailAndPassword,
                IdentityType.PhoneAndPassword
            };

            var request = new IdentitiesGetPagedListRequest
            {
                ProfileId = profile.Id,
                Types = identityTypes,
                Limit = identityTypes.Count
            };

            var allIdentities = await _identitiesService.GetPagedListAsync(request, ct);
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
                new Claim(ClaimTypes.HomePhone, phone),
            };
        }

        private async Task<List<Claim>> GetApiClaimsAsync(IEnumerable<string> scopes, CancellationToken ct)
        {
            var roles = await _resourcesService.GetRolesByScopesAsync(scopes, ct);

            return roles
                .Select(x => new Claim(ClaimTypes.Role, x))
                .ToList();
        }
    }
}