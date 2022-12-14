using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecSys.Api.Areas.CustomsData.Actions.Get;
using RecSys.Api.CommonDtos;
using RecSys.Customs.Client;
using RecSys.Platform.Data.Providers;
using RecSys.Platform.Dtos;
using Unit = RecSys.Api.CommonDtos.Unit;

namespace RecSys.Api.Areas.CustomsData;

[ApiController]
[Route("v1/customs")]
public class CustomsDataController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDbConnectionsProvider _dbConnectionsProvider;

    public CustomsDataController(IMediator mediator, IDbConnectionsProvider dbConnectionsProvider)
    {
        _mediator = mediator;
        _dbConnectionsProvider = dbConnectionsProvider;
    }

    /// <summary>
    /// Получение информации по экспорту/импорту из ФТС.
    /// </summary>
    /// <param name="request">Запрос с фильтром и пагинацией.</param>
    /// <returns>Массив данных из ФТС и ответ пагинации.</returns>
    [HttpPost("by-filter")]
    [ProducesResponseType(200, Type = typeof(GetCustomsDataResponse))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> GetCustomsData([FromBody] GetCustomsDataRequest request)
    {
        var query =
            @$"select item_type, country, region, unit_type, sum(import_worth_total) import_worth_total, sum(import_netto_total) import_netto_total, sum(import_amount_total) import_amount_total, sum(export_worth_total) export_worth_total, sum(export_netto_total) export_netto_total, sum(export_amount_total) export_amount_total
from customs {request.Filter?.GetWhereClause() ?? " "} group by item_type, country, region, unit_type, import_worth_total, import_netto_total, import_amount_total, export_worth_total, export_netto_total, export_amount_total
limit {request.Pagination.Count}
offset {request.Pagination.Offset}";
        var query2 = @$"select item_type,
       country,
       region,
       unit_type,
       COALESCE((select sum(gross_total) from customs cs where item_type = c.item_type AND cs.direction = false), 0) import_netto_total,
       COALESCE((select sum(gross_total) from customs cs where item_type = c.item_type AND cs.direction = true), 0) export_netto_total,
       COALESCE((select sum(amount_total) from customs cs where item_type = c.item_type AND cs.direction = false), 0) import_amount_total,
       COALESCE((select sum(amount_total) from customs cs where item_type = c.item_type AND cs.direction = true), 0) export_amount_total,
       COALESCE((select sum(worth_total) from customs cs where item_type = c.item_type AND cs.direction = false), 0) import_worth_total,
       COALESCE((select sum(worth_total) from customs cs where item_type = c.item_type AND cs.direction = true), 0) export_worth_total
from customs C {request.Filter?.GetWhereClause() ?? " "}
group by item_type, country, region, unit_type
limit {request.Pagination.Count}
offset {request.Pagination.Offset}";
        var pagQuery = @$"select count(1)
from (select 1
from customs C{" "}{request.Filter?.GetWhereClause() ?? " "}
group by item_type, country, region, unit_type) A";
        var connection = _dbConnectionsProvider.GetConnection();
        var result = await connection.QueryAsync<CustomElementDb2>(query2, request.Filter, commandTimeout: 10000);
        var countries = await connection.QueryAsync<Country>(
            "select * from countries where id = ANY(:Ids)",
            new { Ids = result.Select(q => q.Country).ToArray() });
        var units = await connection.QueryAsync<Unit>(
            "select * from units where id = ANY(:Ids)",
            new { Ids = result.Select(q => q.UnitType).ToArray() });
        var itemTypes = await connection.QueryAsync<ItemType>(
            "select * from item_types where id = ANY(:Ids)",
            new { Ids = result.Select(q => q.ItemType).ToArray() });
        var regions = await connection.QueryAsync<Region>(
            "select * from regions where id = ANY(:Ids)",
            new { Ids = result.Select(q => q.Region).ToArray() });
        var final = result.Select(
            x => new CustomsElement
            {
                Region = regions.First(q => q.Id == x.Region),
                Country = countries.First(q => q.Id == x.Country),
                Export = new TransferAmount()
                {
                    Amount = x.ExportAmountTotal,
                    TradeSum = x.ExportWorthTotal,
                    Gross = x.ExportNettoTotal,
                    Unit = units.FirstOrDefault(q => q.Id == x.UnitType)
                },
                Import = new TransferAmount()
                {
                    Amount = x.ImportAmountTotal,
                    TradeSum = x.ImportWorthTotal,
                    Gross = x.ImportNettoTotal,
                    Unit = units.FirstOrDefault(q => q.Id == x.UnitType)
                },
                ItemType = itemTypes.First(q => long.Parse(q.Id) == x.ItemType)
            });
        var count = await connection.QueryFirstAsync<long>(pagQuery, request.Filter, commandTimeout: 10000);
        return Ok(new GetCustomsDataResponse(final.ToArray(), new PaginationResponse(count)));
    }
}
