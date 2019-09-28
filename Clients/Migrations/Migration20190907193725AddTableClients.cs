using FluentMigrator;

namespace Crm.Identity.Clients.Migrations
{
    [Migration(20190907193725)]
    public class Migration20190907193725AddTableClients : Migration
    {
        public override void Up()
        {
            Create.Table("Clients")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ClientId").AsString(256).NotNullable()
                .WithColumn("ClientSecret").AsString(512).NotNullable()
                .WithColumn("RedirectUriPattern").AsString(2048).NotNullable()
                .WithColumn("IsLocked").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime().Nullable();

            Create.PrimaryKey("PK_Clients_Id").OnTable("Clients")
                .Columns("Id");

            Create.UniqueConstraint("UQ_Clients_ClientId").OnTable("Clients")
                .Columns("ClientId");

            Create.Index("IX_Clients_ClientId").OnTable("Clients")
                .OnColumn("ClientId").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_Clients_CreateDateTime").OnTable("Clients")
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Clients_CreateDateTime").OnTable("Clients");
            Delete.Index("IX_Clients_ClientId").OnTable("Clients");
            Delete.UniqueConstraint("UQ_Clients_ClientId").FromTable("Clients");
            Delete.PrimaryKey("PK_Clients_Id").FromTable("Clients");
            Delete.Table("Clients");
        }
    }
}