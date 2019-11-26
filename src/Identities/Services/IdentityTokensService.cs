using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Identities.Models;
using Crm.Identity.Identities.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Identity.Identities.Services
{
    public class IdentityTokensService : IIdentityTokensService
    {
        private readonly IdentitiesStorage _storage;

        public IdentityTokensService(IdentitiesStorage storage)
        {
            _storage = storage;
        }

        public Task<IdentityToken> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.IdentityTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<Guid> CreateAsync(IdentityToken token, CancellationToken ct)
        {
            var newToken = new IdentityToken
            {
                Id = Guid.NewGuid(),
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