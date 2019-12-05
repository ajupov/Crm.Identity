using FluentMigrator;

namespace Crm.Identity.Areas.RefreshTokens.Migrations
{
    [Migration(20191012202122)]
    public class Migration20191012202122AddTableRefreshTokens : Migration
    {
        public override void Up()
        {
            Create.Table("RefreshTokens")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ProfileId").AsGuid().NotNullable()
                .WithColumn("Value").AsString(128).NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ExpirationDateTime").AsDateTime2().NotNullable()
                .WithColumn("IpAddress").AsString(15).Nullable()
                .WithColumn("UserAgent").AsString(256).Nullable();

            Create.PrimaryKey("PK_RefreshTokens_Id").OnTable("RefreshTokens")
                .Column("Id");

            Create.Index("IX_RefreshTokens_ProfileId").OnTable("RefreshTokens")
                .OnColumn("ProfileId").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_RefreshTokens_Value").OnTable("RefreshTokens")
                .OnColumn("Value").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_RefreshTokens_ProfileId").OnTable("RefreshTokens");
            Delete.Index("IX_RefreshTokens_Value").OnTable("RefreshTokens");
            Delete.PrimaryKey("PK_RefreshTokens_Id").FromTable("RefreshTokens");
            Delete.Table("RefreshTokens");
        }
    }
}