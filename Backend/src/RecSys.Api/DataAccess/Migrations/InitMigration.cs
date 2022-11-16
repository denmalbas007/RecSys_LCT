using System.Text.Json;
using System.Text.Json.Serialization;
using FluentMigrator;
using JetBrains.Annotations;

namespace RecSys.Api.DataAccess.Migrations;

[Migration(0)]
[UsedImplicitly]
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
            .WithColumn("id").AsInt64().Identity().PrimaryKey()
            .WithColumn("username").AsString().Unique()
            .WithColumn("password").AsString()
            .WithColumn("email").AsString()
            .WithColumn("profile_pic_url").AsString().Nullable()
            .WithColumn("first_name").AsString()
            .WithColumn("second_name").AsString()
            .WithColumn("middle_name").AsString().Nullable()
            .WithColumn("role").AsString().ForeignKey("roles", "role");

        Create.Table("refresh_tokens")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt64().ForeignKey("users", "id")
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
        Create.Table("regions")
            .WithColumn("id").AsInt64().PrimaryKey()
            .WithColumn("name").AsString();

        var regions = File.ReadAllText("DataAccess/Migrations/regions_import.json");
        var regs = JsonSerializer.Deserialize<ImportType<long>[]>(regions)!;
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
        var conts = JsonSerializer.Deserialize<ImportType<string>[]>(countries)!;
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

        Create.Table("item_types")
            .WithColumn("id").AsInt64().PrimaryKey()
            .WithColumn("name").AsString();

        var types = File.ReadAllText("DataAccess/Migrations/items_import.json");
        var type = JsonSerializer.Deserialize<ImportType<string>[]>(types)!;
        foreach (var typ in type)
        {
            if (long.TryParse(typ.Code, out var qq))
            {
                Insert.IntoTable("item_types")
                    .Row(
                        new
                        {
                            id = qq,
                            name = typ.Name
                        });
            }
        }

        Create.Table("units")
            .WithColumn("id").AsInt64().PrimaryKey()
            .WithColumn("name").AsString();

        var units = File.ReadAllText("DataAccess/Migrations/units_import.json");
        var uns = JsonSerializer.Deserialize<ImportType<long>[]>(units)!;
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

        Create.Table("customs")
            .WithColumn("id").AsInt64().Identity().PrimaryKey()
            .WithColumn("direction").AsBoolean().Indexed()
            .WithColumn("period").AsDate().Indexed()
            .WithColumn("country").AsString().ForeignKey("countries", "id").Indexed()
            .WithColumn("item_type").AsInt64().ForeignKey("item_types", "id").Indexed()
            .WithColumn("unit_type").AsInt64().Nullable().Indexed()
            .WithColumn("worth_total").AsCustom("numeric(30,5)")
            .WithColumn("gross_total").AsCustom("numeric(30,5)")
            .WithColumn("amount_total").AsCustom("numeric(30,5)")
            .WithColumn("region").AsInt64().ForeignKey("regions", "id").Indexed();
    }
}

public class ImportType<T>
{
    [JsonPropertyName("KOD")]
    public T Code { get; init; } = default!;

    [JsonPropertyName("NAME")]
    public string Name { get; init; } = null!;
}
