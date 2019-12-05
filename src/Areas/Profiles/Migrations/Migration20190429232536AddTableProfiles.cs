using FluentMigrator;

namespace Crm.Identity.Areas.Profiles.Migrations
{
    [Migration(20190429232536)]
    public class Migration20190429232536AddTableProfiles : Migration
    {
        public override void Up()
        {
            Create.Table("Profiles")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("Surname").AsString(256).NotNullable()
                .WithColumn("Name").AsString(256).NotNullable()
                .WithColumn("BirthDate").AsDate().Nullable()
                .WithColumn("Gender").AsByte().Nullable()
                .WithColumn("IsLocked").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_Profiles_Id").OnTable("Profiles")
                .Column("Id");

            Create.Index("IX_Profiles_IsLocked_IsDeleted_CreateDateTime").OnTable("Profiles")
                .OnColumn("IsLocked").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Profiles_IsLocked_IsDeleted_CreateDateTime").OnTable("Profiles");
            Delete.PrimaryKey("PK_Profiles_Id").FromTable("Profiles");
            Delete.Table("Profiles");
        }
    }
}