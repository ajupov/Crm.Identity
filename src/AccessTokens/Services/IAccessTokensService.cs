using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.src.Claims.Models;

namespace Ajupov.Identity.src.AccessTokens.Services
{
    public interface IAccessTokensService
    {
        Task<string> CreateAsync(IEnumerable<Claim> claims, CancellationToken ct);
    }
}