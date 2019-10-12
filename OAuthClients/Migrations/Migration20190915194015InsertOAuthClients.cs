﻿using System;
using Ajupov.Utils.All.Password;
using FluentMigrator;

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
                    ClientSecret = Password.ToPasswordHash("site"),
                    RedirectUriPattern = "http://localhost:3000",
                    IsLocked = false,
                    IsDeleted = false,
                    CreateDateTime = DateTime.UtcNow
                });

            Insert.IntoTable("ClientScopes").Row(
                new
                {
                    Id = Guid.NewGuid(),
                    ClientId = clientId,
                    Value = "all"
                });
        }

        public override void Down()
        {
            Delete.FromTable("OAuthClients").Row(new {ClientId = "site"});
        }
    }
}