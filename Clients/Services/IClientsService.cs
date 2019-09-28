using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Clients.Models;
using Crm.Identity.Clients.Requests;

namespace Crm.Identity.Clients.Services
{
    public interface IClientsService
    {
        Task<Client> GetAsync(Guid id, CancellationToken ct);

        Task<Client> GetByClientIdAsync(string clientId, CancellationToken ct);

        Task<List<Client>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<Client>> GetPagedListAsync(ClientsGetPagedListRequest request, CancellationToken ct);

        Task<Guid> CreateAsync(Client client, CancellationToken ct);

        Task UpdateAsync(Client oldClient, Client client, CancellationToken ct);

        Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct);
    }
}