using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Claims.Models;

namespace Crm.Identity.AccessTokens.Services
{
    public interface IAccessTokensService
    {
        Task<string> CreateAsync(IEnumerable<Claim> claimModels, CancellationToken ct);
    }
}