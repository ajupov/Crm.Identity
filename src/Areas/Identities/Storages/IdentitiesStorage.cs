﻿using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Identity.Areas.Identities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Identity.Areas.Identities.Storages
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