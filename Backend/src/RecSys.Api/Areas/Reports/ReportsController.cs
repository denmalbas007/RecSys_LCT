using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecSys.Api.Areas.Reports.Actions.Create;
using RecSys.Api.Areas.Reports.Actions.Get;
using RecSys.Api.Areas.Reports.Actions.GetList;
using RecSys.Api.Areas.Reports.Dtos;
using RecSys.Platform.Dtos;

namespace RecSys.Api.Areas.Reports;

[ApiController]
[Route("v1/reports")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
        => _mediator = mediator;

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
    {
        await Task.Delay(1);
        return Ok(new CreateReportResponse(1));
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
    [HttpGet("by-ids")]
    [ProducesResponseType(200, Type = typeof(GetReportsBatchResponse))]
    [ProducesResponseType(400, Type = typeof(HttpError))]
    [ProducesResponseType(401, Type = typeof(HttpError))]
    [ProducesResponseType(403, Type = typeof(HttpError))]
    [ProducesResponseType(500, Type = typeof(HttpError))]
    public async Task<IActionResult> GetReportsList([FromQuery] GetReportsBatchRequest request)
    {
        await Task.Delay(1);
        return Ok(
            new GetReportsBatchResponse(
                new[]
                {
                    new ReportMetadata()
                    {
                        CreatedAt = DateTime.UtcNow,
                        ExcelUrl =
                            "https://sun9-22.userapi.com/impg/-OVcTAV-tzADcyfinfKIZLlclfeUTeQYWeYVYA/1ugFJfZeCY8.jpg?size=2280x840&quality=96&sign=918be01f72614d20d711fe2ffdfeedfa&type=album",
                        PdfUrl =
                            "https://sun9-22.userapi.com/impg/-OVcTAV-tzADcyfinfKIZLlclfeUTeQYWeYVYA/1ugFJfZeCY8.jpg?size=2280x840&quality=96&sign=918be01f72614d20d711fe2ffdfeedfa&type=album",
                        Name = "Отчет 22",
                        IsReady = true,
                        Id = 123
                    },
                    new ReportMetadata()
                    {
                        CreatedAt = DateTime.UtcNow.AddMonths(-1),
                        Name = "Отчет 1",
                        IsReady = false,
                        Id = 1
                    }
                }));
    }
}
