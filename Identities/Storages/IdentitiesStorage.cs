using Ajupov.Identity.Identities.Models;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ajupov.Identity.Identities.Storages
{
    public class IdentitiesStorage : Storage
    {
        public IdentitiesStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<Models.Identity> Identities { get; set; }

        public DbSet<IdentityToken> IdentityTokens { get; set; }
    }
}