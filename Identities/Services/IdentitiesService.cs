using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Identities.Helpers;
using Ajupov.Identity.Identities.Models;
using Ajupov.Identity.Identities.Requests;
using Ajupov.Identity.Identities.Storages;
using Ajupov.Utils.All.Password;
using Microsoft.EntityFrameworkCore;

namespace Ajupov.Identity.Identities.Services
{
    public class IdentitiesService : IIdentitiesService
    {
        private readonly IdentitiesStorage _storage;

        public IdentitiesService(IdentitiesStorage storage)
        {
            _storage = storage;
        }

        public Task<Models.Identity> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.Identities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<Models.Identity[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Identities
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync(ct);
        }

        public Task<Models.Identity> GetVerifiedByKeyAndTypesAsync(
            string key,
            IEnumerable<IdentityType> types,
            CancellationToken ct)
        {
            return _storage.Identities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Key == key  && x.IsVerified && types.Contains(x.Type), ct);
        }

        public Task<bool> IsExistByKeyAndTypeAsync(string key, IdentityType type, CancellationToken ct)
        {
            return _storage.Identities
                .AsNoTracking()
                .AnyAsync(x => x.Key == key && x.Type == type, ct);
        }

        public Task<Models.Identity[]> GetPagedListAsync(
            IdentitiesGetPagedListRequest request,
            CancellationToken ct)
        {
            return _storage.Identities
                .AsNoTracking()
                .Where(x =>
                    (request.ProfileId == x.ProfileId) &&
                    (request.Types == null || !request.Types.Any() || request.Types.Contains(x.Type)) &&
                    (!request.IsVerified.HasValue || x.IsVerified == request.IsVerified) &&
                    (!request.MinCreateDateTime.HasValue || x.CreateDateTime >= request.MinCreateDateTime) &&
                    (!request.MaxCreateDateTime.HasValue || x.CreateDateTime <= request.MaxCreateDateTime) &&
                    (!request.MinModifyDateTime.HasValue || x.ModifyDateTime >= request.MinModifyDateTime) &&
                    (!request.MaxModifyDateTime.HasValue || x.ModifyDateTime <= request.MaxModifyDateTime))
                .Sort(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArrayAsync(ct);
        }

        public async Task<Guid> CreateAsync(Models.Identity identity, CancellationToken ct)
        {
            var newIdentity = new Models.Identity
            {
                Id = Guid.NewGuid(),
                ProfileId = identity.ProfileId,
                Type = identity.Type,
                Key = identity.Key,
                PasswordHash = identity.PasswordHash,
                IsVerified = identity.IsVerified,
                CreateDateTime = DateTime.UtcNow
            };

            var entry = await _storage.AddAsync(newIdentity, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public Task UpdateAsync(Models.Identity oldIdentity, Models.Identity identity, CancellationToken ct)
        {
            oldIdentity.IsVerified = oldIdentity.Key != identity.Key;
            oldIdentity.Key = identity.Key;
            oldIdentity.ModifyDateTime = DateTime.UtcNow;

            _storage.Update(oldIdentity);

            return _storage.SaveChangesAsync(ct);
        }

        public Task SetPasswordAsync(Models.Identity identity, string password, CancellationToken ct)
        {
            identity.PasswordHash = Password.ToPasswordHash(password);
            identity.ModifyDateTime = DateTime.UtcNow;

            _storage.Update(identity);

            return _storage.SaveChangesAsync(ct);
        }

        public async Task VerifyAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Identities
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    x.IsVerified = true;
                    x.ModifyDateTime = DateTime.UtcNow;
                }, ct);

            await _storage.SaveChangesAsync(ct);
        }

        public async Task UnverifyAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Identities
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    x.IsVerified = false;
                    x.ModifyDateTime = DateTime.UtcNow;
                }, ct);

            await _storage.SaveChangesAsync(ct);
        }

        public bool IsPasswordCorrect(Models.Identity identity, string password)
        {
            return Password.IsVerifiedPassword(password, identity.PasswordHash);
        }
    }
}