using FluentMigrator;

namespace Crm.Identity.Identities.Migrations
{
    [Migration(20190525175248)]
    public class Migration20190525175248AddTableIdentityTokens : Migration
    {
        public override void Up()
        {
            Create.Table("IdentityTokens")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("IdentityId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ExpirationDateTime").AsDateTime2().NotNullable()
                .WithColumn("UseDateTime").AsDateTime2().Nullable()
                .WithColumn("IpAddress").AsString(15).Nullable()
                .WithColumn("UserAgent").AsString(256).Nullable();

            Create.PrimaryKey("PK_IdentityTokens_Id").OnTable("IdentityTokens")
                .Column("Id");

            Create.ForeignKey("FK_IdentityTokens_IdentityId")
                .FromTable("IdentityTokens").ForeignColumn("IdentityId")
                .ToTable("Identities").PrimaryColumn("Id");

            Create.Index("IX_IdentityTokens_IdentityId_Type_Value").OnTable("IdentityTokens")
                .OnColumn("IdentityId").Ascending()
                .OnColumn("Type").Ascending()
                .OnColumn("Value").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_IdentityTokens_IdentityId_Type_Value").OnTable("IdentityTokens");
            Delete.ForeignKey("FK_IdentityTokens_IdentityId").OnTable("IdentityTokens");
            Delete.PrimaryKey("PK_IdentityTokens_Id").FromTable("IdentityTokens");
            Delete.Table("IdentityTokens");
        }
    }
}