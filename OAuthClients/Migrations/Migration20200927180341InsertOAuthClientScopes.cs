using System;
using FluentMigrator;

namespace Crm.Identity.OAuthClients.Migrations
{
    [Migration(20200927180341)]
    public class Migration20200927180341InsertOAuthClientScopes : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("OAuthClientScopes")
                .Row(new
                {
                    Id = Guid.NewGuid(),
                    OAuthClientId = Guid.Parse("83fa3c54-1bc2-4f49-852c-07c07784cc33"),
                    Value = "openid"
                });

            Insert.IntoTable("OAuthClientScopes")
                .Row(new
                {
                    Id = Guid.NewGuid(),
                    OAuthClientId = Guid.Parse("83fa3c54-1bc2-4f49-852c-07c07784cc33"),
                    Value = "profile"
                });
            Insert.IntoTable("OAuthClientScopes")
                .Row(new
                {
                    Id = Guid.NewGuid(),
                    OAuthClientId = Guid.Parse("83fa3c54-1bc2-4f49-852c-07c07784cc33"),
                    Value = "api"
                });
        }

        public override void Down()
        {
            Delete.FromTable("Resources").Row(new
            {
                OAuthClientId = Guid.Parse("83fa3c54-1bc2-4f49-852c-07c07784cc33"),
                Value = "openid"
            });

            Delete.FromTable("Resources").Row(new
            {
                OAuthClientId = Guid.Parse("83fa3c54-1bc2-4f49-852c-07c07784cc33"),
                Value = "profile"
            });

            Delete.FromTable("Resources").Row(new
            {
                OAuthClientId = Guid.Parse("83fa3c54-1bc2-4f49-852c-07c07784cc33"),
                Value = "api"
            });
        }
    }
}
