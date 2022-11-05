using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecSys.Api.Areas.Auth.Actions;
using RecSys.Api.Areas.Auth.Actions.Authenticate;
using RecSys.Api.Areas.Auth.Actions.Register;
using RecSys.Platform.Dtos;

namespace RecSys.Api.Areas.Auth;

[ApiController]
[Route("v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>
    /// Авторизация пользователя.
    /// </summary>
    /// <param name="request">Логин и пароль.</param>
    /// <returns>JWT и Refresh токен.</returns>
    [HttpPost("authenticate")]
    [ProducesResponseType(200, Type = typeof(AuthenticateResponse))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(404, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> Authenticate([FromQuery] AuthenticateUserRequest request)
        => await MediateOkAsync(request);

    /// <summary>
    /// Регистрация пользователя.
    /// </summary>
    /// <param name="request">Информация для регистрации.</param>
    /// <returns>JWT и Refresh токены.</returns>
    [HttpPost("register")]
    [ProducesResponseType(200, Type = typeof(AuthenticateResponse))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(404, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> Register([FromQuery] RegisterUserRequest request)
        => await MediateOkAsync(request);

    private async Task<IActionResult> MediateOkAsync(object request)
    {
        var response = await _mediator.Send(request, HttpContext.RequestAborted);
        return Ok(response);
    }
}
