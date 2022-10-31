using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecSys.Api.Areas.Filters.Actions.Get.Countries;
using RecSys.Api.Areas.Filters.Actions.Get.ItemTypes;
using RecSys.Api.Areas.Filters.Actions.Get.Regions;

namespace RecSys.Api.Areas.Filters;

[ApiController]
[Route("v1/filters")]
public class FiltersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CancellationToken _cancellationToken;

    public FiltersController(IMediator mediator)
    {
        _mediator = mediator;
        _cancellationToken = HttpContext.RequestAborted;
    }

    /// <summary>
    /// Получить список стран.
    /// </summary>
    /// <returns>Список стран.</returns>
    [HttpGet("countries")]
    [ProducesResponseType(200, Type = typeof(GetCountriesResponse))]
    public async Task<IActionResult> GetCountries()
    {
        var result = await _mediator.Send(new GetCountriesRequest(), _cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Получить список предметов.
    /// </summary>
    /// <returns>Список предметов.</returns>
    [HttpGet("item-types")]
    [ProducesResponseType(200, Type = typeof(GetItemTypesResponse))]
    public async Task<IActionResult> GetItemTypes()
    {
        var result = await _mediator.Send(new GetItemTypesRequest(), _cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Получить список регионов.
    /// </summary>
    /// <returns>Список регионов.</returns>
    [HttpGet("regions")]
    [ProducesResponseType(200, Type = typeof(GetRegionsResponse))]
    public async Task<IActionResult> GetRegions()
    {
        var result = await _mediator.Send(new GetRegionsRequest(), _cancellationToken);
        return Ok(result);
    }
}
