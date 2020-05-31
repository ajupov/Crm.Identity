using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Identity.RefreshTokens.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Identity.RefreshTokens.Storages
{
    public class RefreshTokensStorage : Storage
    {
        public RefreshTokensStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<RefreshTokenClaim> RefreshTokenClaims { get; set; }
    }
}
