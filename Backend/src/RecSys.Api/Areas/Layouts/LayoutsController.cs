using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecSys.Api.Areas.Layouts.Actions.Create;
using RecSys.Api.Areas.Layouts.Actions.Get;
using RecSys.Api.Areas.Layouts.Actions.Update;
using RecSys.Platform.Dtos;

namespace RecSys.Api.Areas.Layouts;

[ApiController]
[Route("v1/layouts")]
public class LayoutsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LayoutsController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>
    /// Создание нового 'шаблона'.
    /// </summary>
    /// <param name="request">Параметры нового 'шаблона'.</param>
    /// <returns>id созданого 'шаблона'.</returns>
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(CreateLayoutResponse))]
    [ProducesResponseType(400, Type = typeof(HttpError))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> CreateLayout([FromBody] CreateLayoutRequest request)
        => await MediateOkAsync(request);

    /// <summary>
    /// Получения списка 'шаблонов'.
    /// </summary>
    /// <param name="request">Список idшников 'шаблонов'.</param>
    /// <returns>Список 'шаблонов'.</returns>
    [HttpGet("by-ids")]
    [ProducesResponseType(200, Type = typeof(GetLayoutsResponse))]
    [ProducesResponseType(400, Type = typeof(HttpError))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> GetLayouts([FromQuery] GetLayoutsRequest request)
        => await MediateOkAsync(request);

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
        => await MediateEmptyAsync(request);

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
