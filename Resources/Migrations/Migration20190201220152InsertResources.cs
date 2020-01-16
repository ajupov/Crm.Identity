using System;
using Crm.Identity.Scopes;
using FluentMigrator;

namespace Crm.Identity.Resources.Migrations
{
    [Migration(20190201220152)]
    public class Migration20190201220152InsertResources : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Resources").Row(
                new
                {
                    Id = Guid.NewGuid(),
                    Name = "OpenID",
                    Scope = ScopeNames.OpenId,
                    Description = "API",
                    Uri = "http://identity.litecrm.org",
                    IsLocked = false,
                    IsDeleted = false,
                    CreateDateTime = DateTime.UtcNow,
                    ModifyDateTime = (DateTime?) null
                });

            Insert.IntoTable("Resources").Row(
                new
                {
                    Id = Guid.NewGuid(),
                    Name = "Profile",
                    Scope = ScopeNames.Profile,
                    Description = "Profile",
                    Uri = "http://identity.litecrm.org",
                    IsLocked = false,
                    IsDeleted = false,
                    CreateDateTime = DateTime.UtcNow,
                    ModifyDateTime = (DateTime?) null
                });

            Insert.IntoTable("Resources").Row(
                new
                {
                    Id = Guid.NewGuid(),
                    Name = "API",
                    Scope = ScopeNames.Api,
                    Description = "API",
                    Uri = "http://api.litecrm.org",
                    IsLocked = false,
                    IsDeleted = false,
                    CreateDateTime = DateTime.UtcNow,
                    ModifyDateTime = (DateTime?) null
                });
        }

        public override void Down()
        {
            Delete.FromTable("Resources").Row(new {Scope = ScopeNames.OpenId});
            Delete.FromTable("Resources").Row(new {Scope = ScopeNames.Profile});
            Delete.FromTable("Resources").Row(new {Scope = ScopeNames.Api});
        }
    }
}