using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.src.Claims.Models;
using Ajupov.Identity.src.Profiles.Models;
using Ajupov.Identity.src.RefreshTokens.Models;

namespace Ajupov.Identity.src.Claims.Services
{
    public interface IClaimsService
    {
        Task<List<Claim>> GetByScopesAsync(IEnumerable<string> scopes, Profile profile, CancellationToken ct);

        Task<List<Claim>> GetByRefreshTokenAsync(RefreshToken refreshToken, Profile profile, CancellationToken ct);
    }
}