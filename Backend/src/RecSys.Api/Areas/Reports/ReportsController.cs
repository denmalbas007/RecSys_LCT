using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecSys.Api.Areas.Reports.Actions.Create;
using RecSys.Api.Areas.Reports.Actions.Get;
using RecSys.Api.Areas.Reports.Actions.GetList;
using RecSys.Platform.Dtos;

namespace RecSys.Api.Areas.Reports;

[ApiController]
[Route("v1/reports")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CancellationToken _cancellationToken;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
        _cancellationToken = HttpContext.RequestAborted;
    }

    /// <summary>
    /// Инициализировать геренацию отчета.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <returns>Id отчета.</returns>
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(CreateReportResponse))]
    [ProducesResponseType(400, Type = typeof(HttpError))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportRequest request)
        => await MediateOkAsync(request);

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
        => await MediateOkAsync(request);

    /// <summary>
    /// Получить набор методанных отчетов по айдишникам.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <returns>Отчет.</returns>
    [HttpGet("by-ids")]
    [ProducesResponseType(200, Type = typeof(GetReportsBatchResponse))]
    [ProducesResponseType(400, Type = typeof(HttpError))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> GetReportsList([FromQuery] GetReportsBatchRequest request)
        => await MediateOkAsync(request);

    private async Task<IActionResult> MediateOkAsync(object request)
    {
        var response = await _mediator.Send(request, _cancellationToken);
        return Ok(response);
    }
}
