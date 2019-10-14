using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.OAuth.Services.Claims.Models;

namespace Ajupov.Identity.OAuth.Services.AccessTokens
{
    public interface IAccessTokensService
    {
        Task<string> CreateAsync(IEnumerable<Claim> claims, CancellationToken ct);
    }
}