using Ajupov.Identity.Profiles.Models;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ajupov.Identity.Profiles.Storages
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