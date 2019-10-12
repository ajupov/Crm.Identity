using Ajupov.Identity.OAuthClients.Models;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ajupov.Identity.OAuthClients.Storages
{
    public class OAuthClientsStorage : Storage
    {
        public OAuthClientsStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<OAuthClient> OAuthClients { get; set; }

        public DbSet<OAuthClientScope> OAuthClientScopes { get; set; }
    }
}