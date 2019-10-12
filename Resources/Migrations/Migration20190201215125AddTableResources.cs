using FluentMigrator;

namespace Ajupov.Identity.Resources.Migrations
{
    [Migration(20190201215125)]
    public class Migration20190201215125AddTableResources : Migration
    {
        public override void Up()
        {
            Create.Table("Resources")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Name").AsString(256).NotNullable()
                .WithColumn("Scope").AsString(64).NotNullable()
                .WithColumn("Description").AsString(2048).Nullable()
                .WithColumn("Uri").AsString(2048).Nullable()
                .WithColumn("IsLocked").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_Resources_Id").OnTable("Resources")
                .Column("Id");

            Create.UniqueConstraint("UQ_Resources_Scope").OnTable("Resources")
                .Columns("Scope");

            Create.Index("IX_Resources_Scope").OnTable("Resources")
                .OnColumn("Scope").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Resources_Scope").OnTable("Resources");
            Delete.UniqueConstraint("UQ_Resources_Scope").FromTable("Resources");
            Delete.PrimaryKey("PK_Resources_Id").FromTable("Resources");
            Delete.Table("Resources");
        }
    }
}