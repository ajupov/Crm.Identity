using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Ajupov.Identity.OAuth.Services.AccessTokens
{
    public interface IAccessTokensService
    {
        Task<string> CreateAsync(IEnumerable<Claim> claims, CancellationToken ct);
    }
}