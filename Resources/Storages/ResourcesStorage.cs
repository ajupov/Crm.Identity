using Ajupov.Identity.Resources.Models;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ajupov.Identity.Resources.Storages
{
    public class ResourcesStorage : Storage
    {
        public ResourcesStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<Resource> Resources { get; set; }
    }
}