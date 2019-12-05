using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.Claims.Models;
using Crm.Identity.Areas.Profiles.Models;
using Crm.Identity.Areas.RefreshTokens.Models;

namespace Crm.Identity.Areas.Claims.Services
{
    public interface IClaimsService
    {
        Task<List<Claim>> GetByScopesAsync(IEnumerable<string> scopes, Profile profile, CancellationToken ct);

        Task<List<Claim>> GetByRefreshTokenAsync(RefreshToken refreshToken, Profile profile, CancellationToken ct);
    }
}