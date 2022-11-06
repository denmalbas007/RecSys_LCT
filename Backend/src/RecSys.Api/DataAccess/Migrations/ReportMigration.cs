using FluentMigrator;

namespace RecSys.Api.DataAccess.Migrations;

[Migration(1)]
public class ReportMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("reports")
            .WithColumn("id").AsInt64().Identity().PrimaryKey()
            .WithColumn("name").AsString()
            .WithColumn("created_at").AsDateTime()
            .WithColumn("is_ready").AsBoolean().WithDefaultValue(false)
            .WithColumn("pdf_url").AsString().Nullable()
            .WithColumn("excel_url").AsString().Nullable()
            .WithColumn("owner").AsInt64().ForeignKey("users", "id");

        Create.Table("reports_data")
            .WithColumn("report_id").AsInt64().ForeignKey("reports", "id")
            .WithColumn("region").AsInt64().ForeignKey("regions", "id")
            .WithColumn("item_type").AsString().ForeignKey("item_types", "id")
            .WithColumn("coefficient").AsFloat();

        Create.Table("layouts")
            .WithColumn("id").AsInt64().Identity().PrimaryKey()
            .WithColumn("name").AsString()
            .WithColumn("updated_at").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("filter").AsCustom("jsonb")
            .WithColumn("owner").AsInt64().ForeignKey("users", "id");
    }
}
