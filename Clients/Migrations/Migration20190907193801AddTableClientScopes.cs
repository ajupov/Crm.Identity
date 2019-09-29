using FluentMigrator;

namespace Ajupov.Identity.Clients.Migrations
{
    [Migration(20190907193801)]
    public class Migration20190907193801AddTableClientScopes : Migration
    {
        public override void Up()
        {
            Create.Table("ClientScopes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ClientId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable();

            Create.PrimaryKey("PK_ClientScopes_Id").OnTable("ClientScopes")
                .Columns("Id");

            Create.ForeignKey("FK_ClientScopes_ClientId")
                .FromTable("ClientScopes").ForeignColumn("ClientId")
                .ToTable("Clients").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_ClientScopes_ClientId").OnTable("ClientScopes")
                .Columns("ClientId", "Value");

            Create.Index("IX_ClientScopes_ClientId").OnTable("ClientScopes")
                .OnColumn("ClientId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ClientScopes_ClientId").OnTable("ClientScopes");
            Delete.UniqueConstraint("UQ_ClientScopes_ClientId").FromTable("ClientScopes");
            Delete.ForeignKey("FK_ClientScopes_ClientId").OnTable("ClientScopes");
            Delete.PrimaryKey("PK_ClientScopes_Id").FromTable("ClientScopes");
            Delete.Table("ClientScopes");
        }
    }
}