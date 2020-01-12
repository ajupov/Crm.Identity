﻿using System;
using FluentMigrator;
using PasswordUtils = Ajupov.Utils.All.Password.Password;

namespace Crm.Identity.OAuthClients.Migrations
{
    [Migration(20200106174644)]
    public class Migration20200106174644InsertLocalOAuthClients : Migration
    {
        public override void Up()
        {
            var clientId = Guid.NewGuid();

            Insert.IntoTable("OAuthClients").Row(
                new
                {
                    Id = clientId,
                    ClientId = "site-local",
                    ClientSecret = PasswordUtils.ToPasswordHash("site-local"),
                    RedirectUriPattern = "http://localhost:9000/Auth/Callback",
                    Audience = "http://localhost:9000",
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
            Delete.FromTable("OAuthClients").Row(new {ClientId = "site-local"});
        }
    }
}