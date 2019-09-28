using System;
using FluentMigrator;

namespace Crm.Identity.Clients.Migrations
{
    [Migration(20190915194015)]
    public class Migration20190915194015InsertClients : Migration
    {
        public override void Up()
        {
            var clientId = Guid.NewGuid();

            Insert.IntoTable("Clients").Row(
                new
                {
                    Id = clientId,
                    ClientId = "spa-site",
                    ClientSecret = "spa-site",
                    RedirectUriPattern = "localhost:3000",
                    IsLocked = false,
                    IsDeleted = false,
                    CreateDateTime = DateTime.UtcNow
                });

            Insert.IntoTable("ClientScopes").Row(
                new
                {
                    Id = Guid.NewGuid(),
                    ClientId = clientId,
                    Value = "products"
                });
        }

        public override void Down()
        {
            Delete.FromTable("Clients").Row(new {ClientId = "spa-site"});
        }
    }
}