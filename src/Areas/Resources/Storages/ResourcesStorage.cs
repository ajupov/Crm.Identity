using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Identity.Areas.Resources.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Identity.Areas.Resources.Storages
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