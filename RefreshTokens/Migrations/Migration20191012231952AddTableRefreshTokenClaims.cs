using FluentMigrator;

namespace Ajupov.Identity.RefreshTokens.Migrations
{
    [Migration(20191012231952)]
    public class Migration20191012231952AddTableRefreshTokenClaims : Migration
    {
        public override void Up()
        {
            Create.Table("RefreshTokenClaims")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("TokenId").AsGuid().NotNullable()
                .WithColumn("Type").AsString(256).NotNullable()
                .WithColumn("Value").AsString(256).NotNullable();

            Create.PrimaryKey("PK_RefreshTokenClaims_Id").OnTable("RefreshTokenClaims")
                .Column("Id");

            Create.ForeignKey("FK_RefreshTokenClaims_TokenId")
                .FromTable("RefreshTokenClaims").ForeignColumn("TokenId")
                .ToTable("RefreshTokens").PrimaryColumn("Id");

            Create.Index("IX_RefreshTokenClaims_TokenId").OnTable("RefreshTokenClaims")
                .OnColumn("TokenId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_RefreshTokenClaims_TokenId").OnTable("RefreshTokenClaims");
            Delete.ForeignKey("FK_RefreshTokenClaims_TokenId").OnTable("RefreshTokenClaims");
            Delete.PrimaryKey("PK_RefreshTokenClaims_Id").FromTable("RefreshTokenClaims");
            Delete.Table("RefreshTokenClaims");
        }
    }
}