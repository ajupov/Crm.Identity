using FluentMigrator;

namespace Crm.Identity.OAuthClients.Migrations
{
    [Migration(20190907193801)]
    public class Migration20190907193801AddTableOAuthClientScopes : Migration
    {
        public override void Up()
        {
            Create.Table("OAuthClientScopes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("OAuthClientId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable();

            Create.PrimaryKey("PK_OAuthClientScopes_Id").OnTable("OAuthClientScopes")
                .Columns("Id");

            Create.ForeignKey("FK_OAuthClientScopes_OAuthClientId")
                .FromTable("OAuthClientScopes").ForeignColumn("OAuthClientId")
                .ToTable("OAuthClients").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_OAuthClientScopes_OAuthClientId").OnTable("OAuthClientScopes")
                .Columns("OAuthClientId", "Value");

            Create.Index("IX_OAuthClientScopes_OAuthClientId").OnTable("OAuthClientScopes")
                .OnColumn("OAuthClientId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_OAuthClientScopes_OAuthClientId").OnTable("OAuthClientScopes");
            Delete.UniqueConstraint("UQ_OAuthClientScopes_OAuthClientId").FromTable("OAuthClientScopes");
            Delete.ForeignKey("FK_OAuthClientScopes_OAuthClientId").OnTable("OAuthClientScopes");
            Delete.PrimaryKey("PK_OAuthClientScopes_Id").FromTable("OAuthClientScopes");
            Delete.Table("OAuthClientScopes");
        }
    }
}