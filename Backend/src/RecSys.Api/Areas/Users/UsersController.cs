using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecSys.Api.Areas.Users.Actions.Get;
using RecSys.Platform.Dtos;

namespace RecSys.Api.Areas.Users;

[ApiController]
[Route("v1/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CancellationToken _cancellationToken;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
        _cancellationToken = HttpContext.RequestAborted;
    }

    /// <summary>
    /// Получить информацию по пользователю.
    /// </summary>
    /// <returns>Пользователь.</returns>
    [HttpGet("profile")]
    [ProducesResponseType(200, Type = typeof(GetUserResponse))]
    [ProducesResponseType(400, Type = typeof(HttpError))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(404, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> GetUser()
    {
        var request = new GetUserRequest(1);
        var response = await _mediator.Send(request, _cancellationToken);
        return Ok(response);
    }
}
