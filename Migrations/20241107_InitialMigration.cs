using FluentMigrator;

namespace TaskManager.Migrations;

[Migration(20241107)]
public class InitialMigration : Migration
{
    public override void Up()
    {
        Create.Table("Tasks")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Title").AsString(255).NotNullable()
            .WithColumn("Description").AsString().Nullable()
            .WithColumn("DueDate").AsDateTime().Nullable()
            .WithColumn("IsCompleted").AsBoolean().NotNullable().WithDefaultValue(false);
    }

    public override void Down()
    {
        Delete.Table("Tasks");
    }
}
