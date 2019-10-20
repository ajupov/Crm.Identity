using System;
using FluentMigrator;
using PasswordUtils = Ajupov.Utils.All.Password.Password;

namespace Ajupov.Identity.OAuthClients.Migrations
{
    [Migration(20190915194015)]
    public class Migration20190915194015InsertOAuthClients : Migration
    {
        public override void Up()
        {
            var clientId = Guid.NewGuid();

            Insert.IntoTable("OAuthClients").Row(
                new
                {
                    Id = clientId,
                    ClientId = "site",
                    ClientSecret = PasswordUtils.ToPasswordHash("site"),
                    RedirectUriPattern = "http://localhost:9000",
                    IsLocked = false,
                    IsDeleted = false,
                    CreateDateTime = DateTime.UtcNow
                });

            Insert.IntoTable("OAuthClientScopes").Row(
                new
                {
                    Id = Guid.NewGuid(),
                    OAuthClientId = clientId,
                    Value = "all"
                });
        }

        public override void Down()
        {
            Delete.FromTable("OAuthClients").Row(new {ClientId = "site"});
        }
    }
}