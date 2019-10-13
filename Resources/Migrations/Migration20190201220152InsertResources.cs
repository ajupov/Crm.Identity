using System;
using Ajupov.Identity.Resources.Models;
using FluentMigrator;

namespace Ajupov.Identity.Resources.Migrations
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
                    Type = (byte) ResourceType.Profile,
                    Name = "All",
                    Scope = "all",
                    Description = "All API",
                    Uri = "http://localhost:9001",
                    IsLocked = false,
                    IsDeleted = false,
                    CreateDateTime = DateTime.UtcNow,
                    ModifyDateTime = (DateTime?) null
                });
        }

        public override void Down()
        {
            Delete.FromTable("Resources").Row(new {Scope = "all"});
        }
    }
}