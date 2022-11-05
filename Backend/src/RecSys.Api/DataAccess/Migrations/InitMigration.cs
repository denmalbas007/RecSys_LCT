using System.Text.Json;
using System.Text.Json.Serialization;
using FluentMigrator;

namespace RecSys.Api.DataAccess.Migrations;

[Migration(0)]
public class InitMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        AuthMigrate();
        CustomsDataMigrate();
    }

    private void AuthMigrate()
    {
        Create.Table("roles")
            .WithColumn("role").AsString().PrimaryKey().Unique()
            .WithColumn("description").AsString()
            .WithColumn("rules").AsCustom("jsonb").WithDefaultValue("{}");

        Create.Table("rules")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("name").AsString().Unique();

        Create.Table("roles_rules")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("rule_id").AsInt64().ForeignKey("rules", "id")
            .WithColumn("role").AsString().ForeignKey("roles", "role");

        Create.Table("users")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("username").AsString().Unique()
            .WithColumn("password").AsString()
            .WithColumn("role").AsString().ForeignKey("roles", "role");

        Create.Table("refresh_tokens")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("user_id").AsGuid().ForeignKey("users", "id")
            .WithColumn("token").AsString()
            .WithColumn("expires_at").AsDateTime()
            .WithColumn("created_at").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("created_by_ip").AsString()
            .WithColumn("revoked_at").AsDateTime().Nullable()
            .WithColumn("revoked_by_ip").AsString().Nullable()
            .WithColumn("replaced_by_token").AsString().Nullable();

        Insert.IntoTable("roles").Row(new { role = "defaultAdmin", description = "SysAdmin" })
            .Row(new { role = "user", description = "User" });
    }

    private void CustomsDataMigrate()
    {
        Create.Table("customs_raw")
            .WithColumn("id").AsInt64().Identity().PrimaryKey()
            .WithColumn("napr").AsBoolean()
            .WithColumn("nastranapr").AsString()
            .WithColumn("tnved").AsString()
            .WithColumn("edizm").AsInt64()
            .WithColumn("stoim").AsDecimal()
            .WithColumn("netto").AsDecimal()
            .WithColumn("kol").AsInt32()
            .WithColumn("region").AsString()
            .WithColumn("region_s").AsString()
            .WithColumn("is_processed").AsBoolean().WithDefaultValue(false);

        Create.Table("regions")
            .WithColumn("id").AsInt64().PrimaryKey()
            .WithColumn("name").AsString();

        var regions = File.ReadAllText("DataAccess/Migrations/regions_import.json");
        var regs = JsonSerializer.Deserialize<ImportType[]>(regions)!;
        foreach (var reg in regs)
        {
            Insert.IntoTable("regions")
                .Row(
                    new
                    {
                        id = reg.Code,
                        name = reg.Name
                    });
        }

        Create.Table("countries")
            .WithColumn("id").AsString().PrimaryKey()
            .WithColumn("name").AsString();

        var countries = File.ReadAllText("DataAccess/Migrations/counties_import.json");
        var conts = JsonSerializer.Deserialize<ImportCType[]>(countries)!;
        foreach (var con in conts)
        {
            Insert.IntoTable("countries")
                .Row(
                    new
                    {
                        id = con.Code,
                        name = con.Name
                    });
        }

        Create.Table("units")
            .WithColumn("id").AsInt64().PrimaryKey()
            .WithColumn("name").AsString();

        var units = File.ReadAllText("DataAccess/Migrations/units_import.json");
        var uns = JsonSerializer.Deserialize<ImportType[]>(units)!;
        foreach (var un in uns)
        {
            Insert.IntoTable("units")
                .Row(
                    new
                    {
                        id = un.Code,
                        name = un.Name
                    });
        }

        // Нужен констрейнт на country + region + item_type?.
        Create.Table("customs")
            .WithColumn("id").AsInt64().Identity().PrimaryKey()
            .WithColumn("item_type").AsString().Indexed()
            .WithColumn("period").AsDate().Indexed()
            .WithColumn("country").AsString().ForeignKey("countries", "id").Indexed()
            .WithColumn("region").AsInt64().ForeignKey("regions", "id").Indexed()
            .WithColumn("unit_type").AsInt64()
            .WithColumn("import_worth_total").AsDecimal()
            .WithColumn("import_netto_total").AsDecimal()
            .WithColumn("import_amount_total").AsInt32()
            .WithColumn("export_worth_total").AsDecimal()
            .WithColumn("export_netto_total").AsDecimal()
            .WithColumn("export_amount_total").AsInt32();
    }
}

public class ImportType
{
    [JsonPropertyName("KOD")]
    public long Code { get; init; }

    [JsonPropertyName("NAME")]
    public string Name { get; init; } = null!;
}

public class ImportCType
{
    [JsonPropertyName("KOD")]
    public string Code { get; init; } = null!;

    [JsonPropertyName("NAME")]
    public string Name { get; init; } = null!;
}
