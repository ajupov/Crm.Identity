using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Claims.Models;
using Ajupov.Identity.Profiles.Models;
using Ajupov.Identity.RefreshTokens.Models;

namespace Ajupov.Identity.RefreshTokens.Services
{
    public interface IRefreshTokensService
    {
        Task<RefreshToken> GetByValueAsync(string value, CancellationToken ct);

        Task<string> CreateAsync(
            IEnumerable<Claim> claims,
            Profile profile,
            string userAgent,
            string ipAddress,
            CancellationToken ct);

        Task SetExpiredAsync(Guid id, CancellationToken ct);
    }
}