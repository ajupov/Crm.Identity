using System;
using Crm.Identity.Scopes;
using FluentMigrator;
using PasswordUtils = Ajupov.Utils.All.Password.Password;

namespace Crm.Identity.OAuthClients.Migrations
{
    [Migration(20200121111223)]
    public class Migration20200121111223InsertHttpClientOAuthClients : Migration
    {
        public override void Up()
        {
            var clientId = Guid.NewGuid();

            Insert.IntoTable("OAuthClients").Row(
                new
                {
                    Id = clientId,
                    ClientId = "http-client",
                    ClientSecret = (string) null,
                    RedirectUriPattern = (string) null,
                    Audience = "http-client",
                    IsLocked = false,
                    IsDeleted = false,
                    CreateDateTime = DateTime.UtcNow
                });

            Insert.IntoTable("OAuthClientScopes").Row(
                new
                {
                    Id = Guid.NewGuid(),
                    OAuthClientId = clientId,
                    Value = ScopeNames.OpenId
                }); 
            
            Insert.IntoTable("OAuthClientScopes").Row(
                new
                {
                    Id = Guid.NewGuid(),
                    OAuthClientId = clientId,
                    Value = ScopeNames.Profile
                });
            
            Insert.IntoTable("OAuthClientScopes").Row(
                new
                {
                    Id = Guid.NewGuid(),
                    OAuthClientId = clientId,
                    Value = ScopeNames.Api
                });
        }

        public override void Down()
        {
            Delete.FromTable("OAuthClients").Row(new {ClientId = "http-client"});
        }
    }
}