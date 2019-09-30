using FluentMigrator;

namespace Ajupov.Identity.Identities.Migrations
{
    [Migration(20190525175236)]
    public class Migration20190525175236AddTableIdentities : Migration
    {
        public override void Up()
        {
            Create.Table("Identities")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ProfileId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString(128).NotNullable()
                .WithColumn("PasswordHash").AsString(512).Nullable()
                .WithColumn("IsVerified").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_Identities_Id").OnTable("Identities")
                .Column("Id");

            Create.UniqueConstraint("UQ_Identities_ProfileId_Key").OnTable("Identities")
                .Columns("ProfileId", "Key");

            Create.Index("IX_Identities_ProfileId_Type_Key").OnTable("Identities")
                .OnColumn("ProfileId").Ascending()
                .OnColumn("Type").Ascending()
                .OnColumn("Key").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Identities_ProfileId_Type_Key").OnTable("Identities");
            Delete.UniqueConstraint("UQ_Identities_ProfileId_Key").FromTable("Identities");
            Delete.PrimaryKey("PK_Identities_Id").FromTable("Identities");
            Delete.Table("Identities");
        }
    }
}