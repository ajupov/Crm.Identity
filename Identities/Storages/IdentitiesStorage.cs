using Crm.Identity.Identities.Models;
using Infrastructure.All.Orm;
using Infrastructure.All.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Identity.Identities.Storages
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