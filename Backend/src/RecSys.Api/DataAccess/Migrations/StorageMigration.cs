using FluentMigrator;

namespace RecSys.Api.DataAccess.Migrations;

[Migration(2)]
public class StorageMigration : ForwardOnlyMigration
{
    public override void Up()
        => Create.Table("storage").WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("bytes").AsString();
}

[Migration(3)]
public class StorageMigration2 : ForwardOnlyMigration
{
    public override void Up()
        => Alter.Table("storage").AddColumn("type").AsString().SetExistingRowsTo("pdf");
}
