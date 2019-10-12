using FluentMigrator;

namespace Ajupov.Identity.OAuthClients.Migrations
{
    [Migration(20190907193725)]
    public class Migration20190907193725AddTableOAuthClients : Migration
    {
        public override void Up()
        {
            Create.Table("OAuthClients")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ClientId").AsString(256).NotNullable()
                .WithColumn("ClientSecret").AsString(512).NotNullable()
                .WithColumn("RedirectUriPattern").AsString(2048).NotNullable()
                .WithColumn("IsLocked").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_OAuthClients_Id").OnTable("OAuthClients")
                .Columns("Id");

            Create.UniqueConstraint("UQ_OAuthClients_ClientId").OnTable("OAuthClients")
                .Columns("ClientId");

            Create.Index("IX_OAuthClients_ClientId").OnTable("OAuthClients")
                .OnColumn("ClientId").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_OAuthClients_CreateDateTime").OnTable("OAuthClients")
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_OAuthClients_CreateDateTime").OnTable("OAuthClients");
            Delete.Index("IX_OAuthClients_ClientId").OnTable("OAuthClients");
            Delete.UniqueConstraint("UQ_OAuthClients_ClientId").FromTable("OAuthClients");
            Delete.PrimaryKey("PK_OAuthClients_Id").FromTable("OAuthClients");
            Delete.Table("OAuthClients");
        }
    }
}