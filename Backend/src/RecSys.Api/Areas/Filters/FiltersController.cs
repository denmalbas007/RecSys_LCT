using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecSys.Api.Areas.Filters.Actions.Get.Countries;
using RecSys.Api.Areas.Filters.Actions.Get.ItemTypes;
using RecSys.Api.Areas.Filters.Actions.Get.Regions;
using RecSys.Api.CommonDtos;
using RecSys.Customs.Client;
using RecSys.Platform.Data.Providers;

namespace RecSys.Api.Areas.Filters;

[ApiController]
[Route("v1/filters")]
public class FiltersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CustomsClient _client;
    private readonly IDbConnectionsProvider _dbConnectionsProvider;
    private readonly IDbTransactionsProvider _dbTransactionsProvider;

    public FiltersController(
        IMediator mediator,
        CustomsClient client,
        IDbConnectionsProvider dbConnectionsProvider,
        IDbTransactionsProvider dbTransactionsProvider)
    {
        _mediator = mediator;
        _client = client;
        _dbConnectionsProvider = dbConnectionsProvider;
        _dbTransactionsProvider = dbTransactionsProvider;
    }

    /// <summary>
    /// Получить список стран.
    /// </summary>
    /// <returns>Список стран.</returns>
    [HttpGet("countries")]
    [ProducesResponseType(200, Type = typeof(GetCountriesResponse))]
    public async Task<IActionResult> GetCountries()
    {
        var result = await _mediator.Send(new GetCountriesRequest(), HttpContext.RequestAborted);
        return Ok(result);
    }

    /// <summary>
    /// Получить список предметов.
    /// </summary>
    /// <returns>Список предметов.</returns>
    [HttpGet("item-types/root")]
    [ProducesResponseType(200, Type = typeof(GetItemTypesResponse))]
    public async Task<IActionResult> GetItemTypesRoot()
    {
        var result = await _client.GetRootItemTypesAsync(HttpContext.RequestAborted);
        return Ok(new GetItemTypesResponse(result));
    }

    /// <summary>
    /// Получить список предметов.
    /// </summary>
    /// <returns>Список предметов.</returns>
    [HttpGet("item-types/{parent}")]
    [ProducesResponseType(200, Type = typeof(GetItemTypesResponse))]
    public async Task<IActionResult> GetItemTypes(string parent)
    {
        var result = await _client.GetItemTypesAsync(parent, HttpContext.RequestAborted);
        return Ok(new GetItemTypesResponse(result));
    }

    /// <summary>
    /// Получить список регионов.
    /// </summary>
    /// <returns>Список регионов.</returns>
    [HttpGet("regions")]
    [ProducesResponseType(200, Type = typeof(GetRegionsResponse))]
    public async Task<IActionResult> GetRegions()
    {
        var query = "select * from regions";
        var connection = _dbConnectionsProvider.GetConnection();
        var result = await connection.QueryAsync<Region>(query, transaction: _dbTransactionsProvider.Current);
        return Ok(new GetRegionsResponse(result.ToArray()));
    }
}
