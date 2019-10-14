using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Claims.Models;
using Ajupov.Identity.Profiles.Models;
using Ajupov.Identity.RefreshTokens.Models;

namespace Ajupov.Identity.Claims.Services
{
    public interface IClaimsService
    {
        Task<List<Claim>> GetByScopesAsync(IEnumerable<string> scopes, Profile profile, CancellationToken ct);

        Task<List<Claim>> GetByRefreshTokenAsync(RefreshToken refreshToken, Profile profile, CancellationToken ct);
    }
}