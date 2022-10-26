using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RecSys.Platform.Data.Dtos;
using static RecSys.Platform.Data.DatabaseEnvironmentConstants;

namespace RecSys.Platform.Data.Factories;

public class PostgresConnectionFactory : IPostgresConnectionFactory
{
    private readonly PostgresqlConnectionOptions _pgConnectionOptions;

    public PostgresConnectionFactory(IConfiguration configuration)
    {
        var optionsFromConf = configuration
            .GetSection(nameof(PostgresConnectionFactory))
            .Get<PostgresqlConnectionOptions>() ?? new PostgresqlConnectionOptions();
        if (!bool.TryParse(configuration[DatabasePooling], out var pooling))
            pooling = optionsFromConf.Pooling;
        if (!int.TryParse(configuration[DatabasePort], out var databasePort))
            databasePort = optionsFromConf.Port;
        if (!int.TryParse(configuration[DatabaseConnectionLifetime], out var lifetime))
            lifetime = optionsFromConf.ConnectionLifetime;
        if (!int.TryParse(configuration[DatabaseMaxPoolSize], out var maxPoolSize))
            maxPoolSize = optionsFromConf.MaxPoolSize;
        if (!int.TryParse(configuration[DatabaseMinPoolSize], out var minPoolSize))
            minPoolSize = optionsFromConf.MinPoolSize;
        _pgConnectionOptions = new PostgresqlConnectionOptions
        {
            Database = configuration[DatabaseName] ?? optionsFromConf.Database,
            Host = configuration[DatabaseHost] ?? optionsFromConf.Host,
            Password = configuration[DatabasePassword] ?? optionsFromConf.Password,
            Pooling = pooling,
            Port = databasePort,
            Username = configuration[DatabaseUser] ?? optionsFromConf.Username,
            ConnectionLifetime = lifetime,
            MaxPoolSize = maxPoolSize,
            MinPoolSize = minPoolSize
        };
    }

    public DbConnection GetConnection()
    {
        var sb = new NpgsqlConnectionStringBuilder
        {
            Database = _pgConnectionOptions.Database,
            Password = _pgConnectionOptions.Password,
            Host = _pgConnectionOptions.Host,
            Pooling = _pgConnectionOptions.Pooling,
            Username = _pgConnectionOptions.Username,
            MaxPoolSize = _pgConnectionOptions.MaxPoolSize,
            MinPoolSize = _pgConnectionOptions.MinPoolSize,
            ConnectionLifetime = _pgConnectionOptions.ConnectionLifetime,
            Port = _pgConnectionOptions.Port
        };
        return new NpgsqlConnection(sb.ToString());
    }
}
