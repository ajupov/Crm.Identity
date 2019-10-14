using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Claims.Models;

namespace Ajupov.Identity.AccessTokens.Services
{
    public interface IAccessTokensService
    {
        Task<string> CreateAsync(IEnumerable<Claim> claims, CancellationToken ct);
    }
}