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
using Claim = Ajupov.Identity.OAuth.Services.Claims.Models.Claim;

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

        public async Task<List<Claim>> GetByScopesAsync(
            IEnumerable<string> scopes,
            Profile profile,
            CancellationToken ct)
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
            var apiClaims = refreshToken.Claims.Select(x => new Claim {Type = x.Type, Value = x.Value});

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
                new Claim {Type = ClaimTypes.NameIdentifier, Value = profile.Id.ToString()},
                new Claim {Type = ClaimTypes.Surname, Value = profile.Surname},
                new Claim {Type = ClaimTypes.Name, Value = profile.Name},
                new Claim {Type = ClaimTypes.DateOfBirth, Value = profile.BirthDate?.ToString("dd.MM.yyyy")},
                new Claim {Type = ClaimTypes.Gender, Value = profile.Gender.ToString().ToLower()},
                new Claim {Type = ClaimTypes.Email, Value = email},
                new Claim {Type = ClaimTypes.HomePhone, Value = phone},
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