using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.Claims.Models;
using Crm.Identity.Areas.Profiles.Models;
using Crm.Identity.Areas.RefreshTokens.Models;

namespace Crm.Identity.Areas.RefreshTokens.Services
{
    public interface IRefreshTokensService
    {
        Task<RefreshToken> GetByValueAsync(string value, CancellationToken ct);

        Task<string> CreateAsync(
            IEnumerable<Claim> claims,
            Profile profile,
            string ipAddress,
            string userAgent,
            CancellationToken ct);

        Task SetExpiredAsync(Guid id, CancellationToken ct);
    }
}