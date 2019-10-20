using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.src.Identities.Extensions;
using Ajupov.Identity.src.Identities.Helpers;
using Ajupov.Identity.src.Identities.Models;
using Ajupov.Identity.src.Identities.Requests;
using Ajupov.Identity.src.Identities.Storages;
using Microsoft.EntityFrameworkCore;
using PasswordUtils = Ajupov.Utils.All.Password.Password;

namespace Ajupov.Identity.src.Identities.Services
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

        public Task<Models.Identity> GetByKeyAndTypesAsync(
            string key,
            IEnumerable<IdentityType> types,
            CancellationToken ct)
        {
            var correctedKey = key.Trim().ToLower();

            return _storage.Identities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Key == correctedKey && types.Contains(x.Type), ct);
        }

        public Task<Models.Identity> GetVerifiedByKeyAndTypesAsync(
            string key,
            IEnumerable<IdentityType> types,
            CancellationToken ct)
        {
            var correctedKey = key.Trim().ToLower();

            return _storage.Identities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Key == correctedKey && x.IsVerified && types.Contains(x.Type), ct);
        }

        public Task<Models.Identity> GetByProfileIdAndTypeAsync(Guid profileId, IdentityType type, CancellationToken ct)
        {
            return _storage.Identities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProfileId == profileId && x.Type == type, ct);
        }

        public Task<bool> IsExistByKeyAndTypeAsync(string key, IdentityType type, CancellationToken ct)
        {
            var correctedKey = key.Trim().ToLower();

            return _storage.Identities
                .AsNoTracking()
                .AnyAsync(x => x.Key == correctedKey && x.Type == type, ct);
        }

        public Task<Models.Identity[]> GetPagedListAsync(IdentitiesGetPagedListRequest request, CancellationToken ct)
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
                Key = identity.Key.Trim().ToLower(),
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
            oldIdentity.IsVerified = oldIdentity.Key != identity.Key.Trim().ToLower();
            oldIdentity.Key = identity.Key.Trim().ToLower();
            oldIdentity.ModifyDateTime = DateTime.UtcNow;

            _storage.Update(oldIdentity);

            return _storage.SaveChangesAsync(ct);
        }

        public Task SetPasswordAsync(Models.Identity identity, string password, CancellationToken ct)
        {
            identity.PasswordHash = PasswordUtils.ToPasswordHash(password);
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
            return PasswordUtils.IsVerifiedPassword(password, identity.PasswordHash);
        }

        public async Task ChangePasswordByProfileIdAsync(Guid profileId, string newPassword, CancellationToken ct)
        {
            var passwordHash = PasswordUtils.ToPasswordHash(newPassword);

            await _storage.Identities
                .Where(x => x.ProfileId == profileId && IdentityTypeExtensions.TypesWithPassword.Contains(x.Type))
                .ForEachAsync(x =>
                {
                    x.PasswordHash = passwordHash;
                    x.ModifyDateTime = DateTime.UtcNow;
                }, ct);
                
            await _storage.SaveChangesAsync(ct);
        }
    }
}