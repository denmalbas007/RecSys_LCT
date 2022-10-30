using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecSys.Api.Areas.CustomsData.Actions.Get;
using RecSys.Platform.Dtos;

namespace RecSys.Api.Areas.CustomsData;

[ApiController]
[Route("v1/customs")]
public class CustomsDataController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CancellationToken _cancellationToken;

    public CustomsDataController(IMediator mediator)
    {
        _mediator = mediator;
        _cancellationToken = HttpContext.RequestAborted;
    }

    /// <summary>
    /// Получение информации по экспорту/импорту из ФТС.
    /// </summary>
    /// <param name="request">Запрос с фильтром и пагинацией.</param>
    /// <returns>Массив данных из ФТС и ответ пагинации.</returns>
    [HttpGet("by-filter")]
    [ProducesResponseType(200, Type = typeof(GetCustomsDataResponse))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> GetCustomsData([FromQuery] GetCustomsDataRequest request)
    {
        var response = await _mediator.Send(request, _cancellationToken);
        return Ok(response);
    }
}
