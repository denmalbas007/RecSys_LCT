using System.Text.Json;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecSys.Api.Areas.Layouts.Actions.Create;
using RecSys.Api.Areas.Layouts.Actions.Get;
using RecSys.Api.Areas.Layouts.Actions.Update;
using RecSys.Api.Areas.Layouts.Dtos;
using RecSys.Platform.Data.Providers;
using RecSys.Platform.Dtos;

namespace RecSys.Api.Areas.Layouts;

[ApiController]
[Route("v1/layouts")]
public class LayoutsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDbConnectionsProvider _dbConnectionsProvider;

    public LayoutsController(IMediator mediator, IDbConnectionsProvider dbConnectionsProvider)
    {
        _mediator = mediator;
        _dbConnectionsProvider = dbConnectionsProvider;
    }

    /// <summary>
    /// Создание нового 'шаблона'.
    /// </summary>
    /// <param name="request">Параметры нового 'шаблона'.</param>
    /// <returns>id созданого 'шаблона'.</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(CreateLayoutResponse))]
    [ProducesResponseType(400, Type = typeof(HttpError))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> CreateLayout([FromBody] CreateLayoutRequest request)
    {
        var userId = long.Parse(HttpContext.User.Identities.FirstOrDefault()?.Claims.First(x => x.Type == "Id").Value!);
        var connection = _dbConnectionsProvider.GetConnection();
        var query = @"insert into layouts (name, filter, owner) values (:Name, :Filter::jsonb, :Owner) returning id";
        var id = await connection.QueryFirstAsync<long>(
            query,
            new
            {
                request.Name,
                Filter = JsonSerializer.Serialize(request.Filter),
                Owner = userId
            });
        return Ok(new CreateLayoutResponse(id));
    }

    /// <summary>
    /// Получения списка 'шаблонов'.
    /// </summary>
    /// <param name="request">Список idшников 'шаблонов'.</param>
    /// <returns>Список 'шаблонов'.</returns>
    [HttpPost("by-ids")]
    [ProducesResponseType(200, Type = typeof(GetLayoutsResponse))]
    [ProducesResponseType(400, Type = typeof(HttpError))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> GetLayouts([FromBody] GetLayoutsRequest request)
    {
        var connection = _dbConnectionsProvider.GetConnection();
        var query = @"select * from layouts where id = ANY(:Ids)";
        var result = await connection.QueryAsync<Layout>(query, new { request.Ids });
        return Ok(new GetLayoutsResponse(result.ToArray()));
    }

    /// <summary>
    /// Обновление существующего 'шаблона'.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <returns>Пустой Content.</returns>
    [HttpPut]
    [ProducesResponseType(200, Type = typeof(UpdateLayoutResponse))]
    [ProducesResponseType(400, Type = typeof(HttpError))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> UpdateLayout([FromBody] UpdateLayoutRequest request)
    {
        var userId = long.Parse(HttpContext.User.Identities.FirstOrDefault()?.Claims.First(x => x.Type == "Id").Value!);
        var connection = _dbConnectionsProvider.GetConnection();
        var query = @"update layouts set name = :Name, filter = :Filter::jsonb, owner = :Owner, updated_at = :UpdatedAt where id = :Id";
        await connection.ExecuteAsync(
            query,
            new
            {
                request.Layout.Name,
                Filter = JsonSerializer.Serialize(request.Layout.FilterOuter),
                Owner = userId,
                request.Layout.Id,
                UpdatedAt = DateTime.UtcNow
            });
        return Ok(new UpdateLayoutResponse(request.Layout.Id));
    }

    private async Task<IActionResult> MediateOkAsync(object request)
    {
        var response = await _mediator.Send(request, HttpContext.RequestAborted);
        return Ok(response);
    }

    private async Task<IActionResult> MediateEmptyAsync(object request)
    {
        await _mediator.Send(request, HttpContext.RequestAborted);
        return Ok(null);
    }
}
