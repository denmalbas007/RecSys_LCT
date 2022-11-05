using Dapper;
using MediatR;
using RecSys.Api.CommonDtos;
using RecSys.Platform.Data.Providers;

namespace RecSys.Api.Areas.Filters.Actions.Get.Countries;

public class GetCountriesHandler : IRequestHandler<GetCountriesRequest, GetCountriesResponse>
{
    private readonly IDbConnectionsProvider _dbConnectionsProvider;
    private readonly IDbTransactionsProvider _dbTransactionsProvider;

    public GetCountriesHandler(IDbConnectionsProvider dbConnectionsProvider, IDbTransactionsProvider dbTransactionsProvider)
    {
        _dbConnectionsProvider = dbConnectionsProvider;
        _dbTransactionsProvider = dbTransactionsProvider;
    }

    public async Task<GetCountriesResponse> Handle(GetCountriesRequest request, CancellationToken cancellationToken)
    {
        var query = "select * from countries";
        var connection = _dbConnectionsProvider.GetConnection();
        var result = await connection.QueryAsync<Country>(query, transaction: _dbTransactionsProvider.Current);
        return new GetCountriesResponse(result.ToArray());
    }
}
