using Microsoft.Extensions.DependencyInjection;
using RecSys.Platform.Data.Factories;
using RecSys.Platform.Data.Providers;

namespace RecSys.Platform.Data.Extensions;

public static class DataAccessExtensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services)
        => services
            .AddScoped<IPostgresConnectionFactory, PostgresConnectionFactory>()
            .AddScoped<IDbConnectionsProvider, DbConnectionsProvider>()
            .AddScoped<IDbTransactionsProvider, DbTransactionsProvider>();
}
