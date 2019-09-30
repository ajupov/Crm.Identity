using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Identities.Models;
using Ajupov.Identity.Identities.Storages;
using Microsoft.EntityFrameworkCore;

namespace Ajupov.Identity.Identities.Services
{
    public class IdentityTokensService : IIdentityTokensService
    {
        private readonly IdentitiesStorage _storage;

        public IdentityTokensService(IdentitiesStorage storage)
        {
            _storage = storage;
        }

        public Task<IdentityToken> GetAsync(Guid identityId, string value, CancellationToken ct)
        {
            return _storage.IdentityTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdentityId == identityId && x.Value == value, ct);
        }

        public Task<IdentityToken> GetByValueAsync(IdentityTokenType type, string value, CancellationToken ct)
        {
            return _storage.IdentityTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Type == type && x.Value == value, ct);
        }

        public async Task<Guid> CreateAsync(IdentityToken token, CancellationToken ct)
        {
            var newToken = new IdentityToken
            {
                IdentityId = token.IdentityId,
                Type = token.Type,
                Value = token.Value,
                CreateDateTime = DateTime.UtcNow,
                ExpirationDateTime = token.ExpirationDateTime,
                UserAgent = token.UserAgent,
                IpAddress = token.IpAddress
            };

            var entry = await _storage.AddAsync(newToken, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task SetIsUsedAsync(Guid id, CancellationToken ct)
        {
            await _storage.IdentityTokens
                .Where(x => x.Id == id)
                .ForEachAsync(x => x.UseDateTime = DateTime.UtcNow, ct);

            await _storage.SaveChangesAsync(ct);
        }
    }
}