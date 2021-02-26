using System;
using Crm.Identity.Resources.Models;
using FluentMigrator;

namespace Crm.Identity.Resources.Migrations
{
    [Migration(20200927174052)]
    public class Migration20200927174052InsertResources : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Resources")
                .Row(new
                {
                    Id = Guid.NewGuid(),
                    Name = "OpenID",
                    Scope = "openid",
                    Description = "OpenID",
                    Uri = "https://identity.litecrm.org",
                    IsLocked = false,
                    IsDeleted = false,
                    CreateDateTime = DateTime.UtcNow,
                    ModifyDateTime = (DateTime?) null
                });

            Insert.IntoTable("Resources")
                .Row(new
                {
                    Id = Guid.NewGuid(),
                    Name = "Profile",
                    Scope = "profile",
                    Description = "Profile",
                    Uri = "https://identity.litecrm.org",
                    IsLocked = false,
                    IsDeleted = false,
                    CreateDateTime = DateTime.UtcNow,
                    ModifyDateTime = (DateTime?) null
                });

            Insert.IntoTable("Resources")
                .Row(new
                {
                    Id = Guid.NewGuid(),
                    Name = "API",
                    Scope = "api",
                    Description = "API",
                    Uri = "https://api.litecrm.org",
                    IsLocked = false,
                    IsDeleted = false,
                    CreateDateTime = DateTime.UtcNow,
                    ModifyDateTime = (DateTime?) null
                });
        }

        public override void Down()
        {
            Delete.FromTable("Resources").Row(new {Scope = "openid"});
            Delete.FromTable("Resources").Row(new {Scope = "profile"});
            Delete.FromTable("Resources").Row(new {Scope = "api"});
        }
    }
}
