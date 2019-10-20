using System;
using FluentMigrator;

namespace Ajupov.Identity.src.Resources.Migrations
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
                    Name = "All",
                    Scope = "all",
                    Description = "All API",
                    Uri = "http://localhost:9000",
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