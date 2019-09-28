using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Clients.Helpers;
using Crm.Identity.Clients.Models;
using Crm.Identity.Clients.Requests;
using Crm.Identity.Clients.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Identity.Clients.Services
{
    public class ClientsService : IClientsService
    {
        private readonly ClientsStorage _storage;

        public ClientsService(ClientsStorage storage)
        {
            _storage = storage;
        }

        public Task<Client> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.Clients
                .Include(x => x.Scopes)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<Client> GetByClientIdAsync(string clientId, CancellationToken ct)
        {
            return _storage.Clients
                .Include(x => x.Scopes)
                .FirstOrDefaultAsync(x => x.ClientId == clientId, ct);
        }

        public Task<List<Client>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Clients
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public Task<List<Client>> GetPagedListAsync(ClientsGetPagedListRequest request, CancellationToken ct)
        {
            return _storage.Clients
                .Where(x =>
                    (!request.IsLocked.HasValue || x.IsLocked == request.IsLocked) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .Sort(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Client client, CancellationToken ct)
        {
            var clientId = Guid.NewGuid();

            var newClient = new Client
            {
                Id = clientId,
                ClientId = client.ClientId,
                ClientSecret = client.ClientSecret,
                RedirectUriPattern = client.RedirectUriPattern,
                IsLocked = client.IsLocked,
                IsDeleted = client.IsDeleted,
                CreateDateTime = DateTime.UtcNow,
                Scopes = client.Scopes?.Select(x => new ClientScope
                {
                    Id = Guid.NewGuid(),
                    ClientId = clientId,
                    Value = x.Value
                }).ToList()
            };

            var entry = await _storage.AddAsync(newClient, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public Task UpdateAsync(Client oldClient, Client client, CancellationToken ct)
        {
            oldClient.ClientId = client.ClientId;
            oldClient.ClientSecret = client.ClientSecret;
            oldClient.RedirectUriPattern = client.RedirectUriPattern;
            oldClient.IsLocked = client.IsLocked;
            oldClient.IsDeleted = client.IsDeleted;
            oldClient.ModifyDateTime = DateTime.UtcNow;
            oldClient.Scopes = client.Scopes
                .Select(s => new ClientScope
                {
                    Id = oldClient.Id,
                    ClientId = s.ClientId,
                    Value = s.Value
                })
                .ToList();

            _storage.Update(oldClient);

            return _storage.SaveChangesAsync(ct);
        }

        public async Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Clients
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    x.IsLocked = true;
                    x.ModifyDateTime = DateTime.UtcNow;
                }, ct);

            await _storage.SaveChangesAsync(ct);
        }

        public async Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Clients
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    x.IsLocked = false;
                    x.ModifyDateTime = DateTime.UtcNow;
                }, ct);

            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Clients
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    x.IsDeleted = true;
                    x.ModifyDateTime = DateTime.UtcNow;
                }, ct);

            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Clients
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    x.IsDeleted = false;
                    x.ModifyDateTime = DateTime.UtcNow;
                }, ct);

            await _storage.SaveChangesAsync(ct);
        }
    }
}