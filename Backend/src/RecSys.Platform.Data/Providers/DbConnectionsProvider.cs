using System.Data.Common;
using RecSys.Platform.Data.Factories;

namespace RecSys.Platform.Data.Providers;

public class DbConnectionsProvider : IDbConnectionsProvider, IAsyncDisposable, IDisposable
{
    private readonly IPostgresConnectionFactory _postgresConnectionFactory;
    private DbConnection? _connection;

    public DbConnectionsProvider(IPostgresConnectionFactory postgresConnectionFactory)
        => _postgresConnectionFactory = postgresConnectionFactory;

    public DbConnection GetConnection()
    {
        _connection ??= _postgresConnectionFactory.GetConnection();
        return _connection;
    }

    public ValueTask DisposeAsync()
    {
        _connection?.Dispose();
        return ValueTask.CompletedTask;
    }

    public void Dispose()
        => _connection?.Dispose();
}
