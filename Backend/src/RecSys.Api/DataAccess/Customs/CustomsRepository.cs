using Dapper;
using RecSys.Api.CommonDtos;
using RecSys.Platform.Data.Providers;

namespace RecSys.Api.DataAccess.Customs;

public class CustomsRepository
{
    private readonly IDbConnectionsProvider _dbConnectionsProvider;
    private readonly IDbTransactionsProvider _dbTransactionsProvider;

    public CustomsRepository(IDbConnectionsProvider dbConnectionsProvider, IDbTransactionsProvider dbTransactionsProvider)
    {
        _dbConnectionsProvider = dbConnectionsProvider;
        _dbTransactionsProvider = dbTransactionsProvider;
    }

    // TODO: Грязь. Потом отрефачить.
    public async Task<(CustomsElement[] customsElement, PaginationResponse paginationResponse)> GetCustomsElementsByFilter(
        Filter filter,
        Pagination pagination,
        CancellationToken cancellationToken)
    {
        const string query = "";
        const string paginationQuery = "";
        var connection = _dbConnectionsProvider.GetConnection();
        var transaction = _dbTransactionsProvider.Current;
        var result = await connection.QueryAsync<CustomsElement>(query, filter, transaction);
        var paginationResult = await connection.QueryFirstAsync<long>(paginationQuery, filter, transaction);
        return (result.ToArray(), new PaginationResponse(paginationResult));
    }
}
