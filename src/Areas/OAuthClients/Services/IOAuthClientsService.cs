using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.OAuthClients.Models;
using Crm.Identity.Areas.OAuthClients.Requests;

namespace Crm.Identity.Areas.OAuthClients.Services
{
    public interface IOAuthClientsService
    {
        Task<OAuthClient> GetAsync(Guid id, CancellationToken ct);

        Task<OAuthClient> GetByClientIdAsync(string clientId, CancellationToken ct);

        Task<List<OAuthClient>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<OAuthClient>> GetPagedListAsync(OAuthClientsGetPagedListRequest request, CancellationToken ct);

        Task<Guid> CreateAsync(OAuthClient oAuthClient, CancellationToken ct);

        Task UpdateAsync(OAuthClient oldOAuthClient, OAuthClient oAuthClient, CancellationToken ct);

        Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct);
    }
}