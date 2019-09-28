using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Identity.Clients.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Identity.Clients.Storages
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