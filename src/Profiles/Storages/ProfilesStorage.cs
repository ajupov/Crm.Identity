using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Identity.Profiles.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Identity.Profiles.Storages
{
    public class ProfilesStorage : Storage
    {
        public ProfilesStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<Profile> Profiles { get; set; }
    }
}