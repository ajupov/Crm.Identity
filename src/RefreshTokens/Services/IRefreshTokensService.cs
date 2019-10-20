using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.src.Claims.Models;
using Ajupov.Identity.src.Profiles.Models;
using Ajupov.Identity.src.RefreshTokens.Models;

namespace Ajupov.Identity.src.RefreshTokens.Services
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