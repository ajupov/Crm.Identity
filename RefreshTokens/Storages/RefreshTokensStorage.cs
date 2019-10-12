using Ajupov.Identity.RefreshTokens.Models;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ajupov.Identity.RefreshTokens.Storages
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