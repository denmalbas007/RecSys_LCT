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
        var query = @"select * from countries where id in ('US', 'AU', 'AL', 'UK', 'CA', 'NO', 'KR', 'SG', 'TW', 'UA', 'CH', 'ME', 'JP', 'AT', 'BE',
        'BG', 'HU', 'DE', 'GR', 'DK', 'ES', 'IT', 'LV', 'LT', 'NL', 'PL', 'PT', 'RO', 'SK', 'SL',
        'FI', 'FR', 'HR', 'CZ', 'SE', 'EE')";
        var connection = _dbConnectionsProvider.GetConnection();
        var result = await connection.QueryAsync<Country>(query, transaction: _dbTransactionsProvider.Current);
        return new GetCountriesResponse(result.ToArray());
    }
}
