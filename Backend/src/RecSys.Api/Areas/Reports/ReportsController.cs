using System.Text.Json;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecSys.Api.Areas.Reports.Actions.Create;
using RecSys.Api.Areas.Reports.Actions.Get;
using RecSys.Api.Areas.Reports.Actions.GetList;
using RecSys.Api.Areas.Reports.Dtos;
using RecSys.Platform.Data.Providers;
using RecSys.Platform.Dtos;

namespace RecSys.Api.Areas.Reports;

[ApiController]
[Route("v1/reports")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDbConnectionsProvider _dbConnectionsProvider;

    public ReportsController(IMediator mediator, IDbConnectionsProvider dbConnectionsProvider)
    {
        _mediator = mediator;
        _dbConnectionsProvider = dbConnectionsProvider;
    }

    /// <summary>
    /// Инициализировать геренацию отчета.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <returns>Id отчета.</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(CreateReportResponse))]
    [ProducesResponseType(400, Type = typeof(HttpError))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportRequest request)
    {
        var userId = long.Parse(HttpContext.User.Identities.FirstOrDefault()?.Claims.First(x => x.Type == "Id").Value!);
        var query = @"insert into reports (name, owner, filter, created_at) values (:Name, :Owner, :Filter::jsonb, :DateTime)
returning id";
        var connection = _dbConnectionsProvider.GetConnection();
        var id = await connection.QueryFirstAsync<long>(
            query,
            new
            {
                request.Name,
                Owner = userId,
                Filter = JsonSerializer.Serialize(request.Filter),
                DateTime = DateTime.UtcNow
            });
        return Ok(new CreateReportResponse(id));
    }

    /// <summary>
    /// Получить отчет.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <returns>Отчет.</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(GetReportResponse))]
    [ProducesResponseType(400, Type = typeof(HttpError))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(404, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> GetReport([FromQuery] GetReportRequest request)
    {
        await Task.Delay(1);
        return NoContent();
    }

    /// <summary>
    /// Получить набор методанных отчетов по айдишникам.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <returns>Отчет.</returns>
    [HttpPost("by-ids")]
    [ProducesResponseType(200, Type = typeof(GetReportsBatchResponse))]
    [ProducesResponseType(400, Type = typeof(HttpError))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> GetReportsList([FromBody] GetReportsBatchRequest request)
    {
        await Task.Delay(1);
        var userId = long.Parse(HttpContext.User.Identities.FirstOrDefault()?.Claims.First(x => x.Type == "Id").Value!);
        var query = @"select * from reports where id = ANY(:Ids)";
        var connection = _dbConnectionsProvider.GetConnection();
        var meta = await connection.QueryAsync<ReportMetadata>(
            query,
            new
            {
                request.Ids,
            });
        return Ok(new GetReportsBatchResponse(meta.ToArray()));
    }
}
