using Ajupov.Identity.Clients.Models;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ajupov.Identity.Clients.Storages
{
    public class ClientsStorage : Storage
    {
        public ClientsStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<ClientScope> ClientScopes { get; set; }
    }
}