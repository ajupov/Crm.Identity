using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.Claims.Models;

namespace Crm.Identity.Areas.AccessTokens.Services
{
    public interface IAccessTokensService
    {
        Task<string> CreateAsync(IEnumerable<Claim> claims, CancellationToken ct);
    }
}