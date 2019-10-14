using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.OAuth.Services.Claims.Models;
using Ajupov.Identity.Profiles.Models;
using Ajupov.Identity.RefreshTokens.Models;

namespace Ajupov.Identity.OAuth.Services.Claims
{
    public interface IClaimsService
    {
        Task<List<Claim>> GetByScopesAsync(IEnumerable<string> scopes, Profile profile, CancellationToken ct);

        Task<List<Claim>> GetByRefreshTokenAsync(RefreshToken refreshToken, Profile profile, CancellationToken ct);
    }
}