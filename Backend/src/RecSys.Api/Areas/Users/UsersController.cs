using Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecSys.Api.Areas.Users.Actions.Get;
using RecSys.Api.Areas.Users.Dtos;
using RecSys.Platform.Data.Providers;
using RecSys.Platform.Dtos;

namespace RecSys.Api.Areas.Users;

[ApiController]
[Route("v1/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDbConnectionsProvider _dbConnectionsProvider;

    public UsersController(IMediator mediator, IDbConnectionsProvider dbConnectionsProvider)
    {
        _mediator = mediator;
        _dbConnectionsProvider = dbConnectionsProvider;
    }

    /// <summary>
    /// Получить информацию по пользователю.
    /// </summary>
    /// <returns>Пользователь.</returns>
    [HttpGet("profile")]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(GetUserResponse))]
    [ProducesResponseType(400, Type = typeof(HttpError))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(404, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> GetUser()
    {
        var userId = long.Parse(HttpContext.User.Identities.FirstOrDefault()?.Claims.First(x => x.Type == "Id").Value!);
        var connection = _dbConnectionsProvider.GetConnection();
        var query = @"select * from users where id = :Id";
        var resultU = await connection.QueryFirstOrDefaultAsync<User>(query, new { Id = userId });
        var reportsQuery = @"select id from reports where owner = :Id";
        var layoutsQuery = @"select id from layouts where owner = :Id";
        resultU.ReportIds = (await connection.QueryAsync<long>(reportsQuery, new { Id = userId })).ToArray();
        resultU.LayoutIds = (await connection.QueryAsync<long>(layoutsQuery, new { Id = userId })).ToArray();
        return Ok(new GetUserResponse(resultU));
    }
}
