using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.OAuthClients.Helpers;
using Crm.Identity.OAuthClients.Models;
using Crm.Identity.OAuthClients.Requests;
using Crm.Identity.OAuthClients.Storages;
using Microsoft.EntityFrameworkCore;
using PasswordUtils = Ajupov.Utils.All.Password.Password;

namespace Crm.Identity.OAuthClients.Services
{
    public class OAuthClientsService : IOAuthClientsService
    {
        private readonly OAuthClientsStorage _storage;

        public OAuthClientsService(OAuthClientsStorage storage)
        {
            _storage = storage;
        }

        public Task<OAuthClient> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.OAuthClients
                .AsNoTracking()
                .Include(x => x.Scopes)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<OAuthClient> GetByClientIdAsync(string clientId, CancellationToken ct)
        {
            return _storage.OAuthClients
                .AsNoTracking()
                .Include(x => x.Scopes)
                .FirstOrDefaultAsync(x => x.ClientId == clientId, ct);
        }

        public Task<List<OAuthClient>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.OAuthClients
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public Task<List<OAuthClient>> GetPagedListAsync(OAuthClientsGetPagedListRequest request, CancellationToken ct)
        {
            return _storage.OAuthClients
                .AsNoTracking()
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

        public async Task<Guid> CreateAsync(OAuthClient oAuthClient, CancellationToken ct)
        {
            var clientId = Guid.NewGuid();

            var newOAuthClient = new OAuthClient
            {
                Id = clientId,
                ClientId = oAuthClient.ClientId,
                ClientSecret = PasswordUtils.ToPasswordHash(oAuthClient.ClientSecret),
                RedirectUriPattern = oAuthClient.RedirectUriPattern,
                IsLocked = oAuthClient.IsLocked,
                IsDeleted = oAuthClient.IsDeleted,
                CreateDateTime = DateTime.UtcNow,
                Scopes = oAuthClient.Scopes?.Select(x => new OAuthClientScope
                {
                    Id = Guid.NewGuid(),
                    OAuthClientId = clientId,
                    Value = x.Value
                }).ToList()
            };

            var entry = await _storage.AddAsync(newOAuthClient, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public Task UpdateAsync(OAuthClient oldOAuthClient, OAuthClient oAuthClient, CancellationToken ct)
        {
            oldOAuthClient.ClientId = oAuthClient.ClientId;
            oldOAuthClient.ClientSecret = PasswordUtils.ToPasswordHash(oAuthClient.ClientSecret);
            oldOAuthClient.RedirectUriPattern = oAuthClient.RedirectUriPattern;
            oldOAuthClient.IsLocked = oAuthClient.IsLocked;
            oldOAuthClient.IsDeleted = oAuthClient.IsDeleted;
            oldOAuthClient.ModifyDateTime = DateTime.UtcNow;
            oldOAuthClient.Scopes = oAuthClient.Scopes
                .Select(s => new OAuthClientScope
                {
                    Id = oldOAuthClient.Id,
                    OAuthClientId = s.OAuthClientId,
                    Value = s.Value
                })
                .ToList();

            _storage.Update(oldOAuthClient);

            return _storage.SaveChangesAsync(ct);
        }

        public async Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.OAuthClients
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
            await _storage.OAuthClients
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
            await _storage.OAuthClients
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
            await _storage.OAuthClients
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