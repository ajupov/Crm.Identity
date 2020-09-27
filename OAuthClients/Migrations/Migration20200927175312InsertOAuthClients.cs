using System;
using FluentMigrator;

namespace Crm.Identity.OAuthClients.Migrations
{
    [Migration(20200927175312)]
    public class Migration20200927175312InsertOAuthClients : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("OAuthClients")
                .Row(new
                {
                    Id = Guid.Parse("83fa3c54-1bc2-4f49-852c-07c07784cc33"),
                    ClientId = "site-local",
                    ClientSecret = Ajupov.Utils.All.Password.Password.ToPasswordHash("site-local"),
                    RedirectUriPattern = "http://localhost:9000/Auth/Callback",
                    Audience = "localhost:9000",
                    IsLocked = false,
                    IsDeleted = false,
                    CreateDateTime = DateTime.UtcNow,
                    ModifyDateTime = (DateTime?) null
                });
        }

        public override void Down()
        {
            Delete.FromTable("Resources").Row(new {Id = Guid.Parse("83fa3c54-1bc2-4f49-852c-07c07784cc33")});
        }
    }
}
