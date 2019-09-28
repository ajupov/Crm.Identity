using Crm.Identity.Profiles.Models;
using Infrastructure.All.Orm;
using Infrastructure.All.Orm.Settings;
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