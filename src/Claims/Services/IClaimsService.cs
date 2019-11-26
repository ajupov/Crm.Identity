using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Claims.Models;
using Crm.Identity.Profiles.Models;
using Crm.Identity.RefreshTokens.Models;

namespace Crm.Identity.Claims.Services
{
    public interface IClaimsService
    {
        Task<List<Claim>> GetByScopesAsync(IEnumerable<string> scopes, Profile profile, CancellationToken ct);

        Task<List<Claim>> GetByRefreshTokenAsync(RefreshToken refreshToken, Profile profile, CancellationToken ct);
    }
}