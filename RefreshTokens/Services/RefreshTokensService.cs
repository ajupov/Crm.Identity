using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Generator;
using Crm.Identity.Claims.Models;
using Crm.Identity.Profiles.Models;
using Crm.Identity.RefreshTokens.Models;
using Crm.Identity.RefreshTokens.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Identity.RefreshTokens.Services
{
    public class RefreshTokensService : IRefreshTokensService
    {
        private readonly RefreshTokensStorage _storage;

        public RefreshTokensService(RefreshTokensStorage storage)
        {
            _storage = storage;
        }

        public Task<RefreshToken> GetByValueAsync(string value, CancellationToken ct)
        {
            return _storage.RefreshTokens
                .Include(x => x.Claims)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Value == value, ct);
        }

        public async Task<string> CreateAsync(
            IEnumerable<Claim> claims,
            Profile profile,
            string ipAddress,
            string userAgent,
            CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();

            var refreshToken = new RefreshToken
            {
                Id = id,
                ProfileId = profile.Id,
                Value = Generator.GenerateAlphaNumericString(128),
                Claims = claims
                    .Select(x => new RefreshTokenClaim
                    {
                        Id = Guid.NewGuid(),
                        RefreshTokenId = id,
                        Type = x.Type,
                        Value = x.Value
                    }).ToList(),
                CreateDateTime = now,
                ExpirationDateTime = now.AddDays(60),
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            var entry = await _storage.AddAsync(refreshToken, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Value;
        }

        public Task SetExpiredAsync(Guid id, CancellationToken ct)
        {
            return _storage.RefreshTokens
                .Where(x => x.Id == id)
                .ForEachAsync(x => x.ExpirationDateTime = DateTime.UtcNow, ct);
        }
    }
}